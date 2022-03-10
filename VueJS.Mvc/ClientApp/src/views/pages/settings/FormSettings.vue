<template>
    <b-row>
        <b-col cols="12">
            <b-row class="content-header">
                <b-col class="content-header-left mb-2"
                       cols="12"
                       md="8">
                    <h2 class="content-header-title float-left pr-1 mb-0">
                        Form Ayarları
                    </h2>
                    <div class="breadcrumb-wrapper">
                        <b-breadcrumb>
                            <b-breadcrumb-item to="/">
                                <feather-icon icon="HomeIcon"
                                              size="16"
                                              class="align-text-top" />
                            </b-breadcrumb-item>
                            <b-breadcrumb-item active>Form Ayarlarını Düzenle</b-breadcrumb-item>
                        </b-breadcrumb>
                    </div>
                </b-col>
                <b-col class="content-header-right text-md-right d-md-block d-none mb-1"
                       md="4"
                       cols="12">
                    <b-button v-if="$can('update', 'Seo')"
                              variant="primary"
                              type="button"
                              :disabled="overlayShow"
                              @click.prevent="updateData">
                        Güncelle
                    </b-button>
                </b-col>
            </b-row>
            <b-overlay :show="overlayShow"
                       rounded="sm"
                       opacity="0.80"
                       variant="white"
                       no-fade>
                <b-row>
                    <b-col cols="12">
                        <b-tabs id="tab-form-settings"
                                vertical
                                content-class="col-12 col-md-9 mt-1 mt-md-0"
                                pills
                                nav-wrapper-class="col-md-3 col-12"
                                nav-class="nav-left">
                            <b-tab active>
                                <template #title>
                                    <feather-icon icon="MailIcon"
                                                  size="18"
                                                  class="mr-50" />
                                    <span class="font-weight-bold">E-Posta</span>
                                </template>
                                <b-card id="card-email"
                            header-tag="header">
                                    <template #header>
                                        <span class="font-weight-bold" style="font-size:18px;">E-Posta Ayarları</span>
                                    </template>
                                    <b-row>
                                        <b-col cols="12">
                                            <b-form-group label="Gönderen Bilgisi"
                                                          label-for="senderEmail">
                                                <b-form-input id="senderEmail"
                                                              v-model="smtpSettings.SenderEmail"
                                                              placeholder="Gönderen bilgisini giriniz" />
                                            </b-form-group>
                                        </b-col>
                                        <b-col lg="6"
                                               md="6"
                                               sm="12">
                                            <b-form-group label="Sunucu"
                                                          label-for="server">
                                                <b-form-input id="server"
                                                              v-model="smtpSettings.Server"
                                                              placeholder="Sunucu bilgisini giriniz" />
                                            </b-form-group>
                                        </b-col>
                                        <b-col lg="6"
                                               md="6"
                                               sm="12">
                                            <b-form-group label="Port"
                                                          label-for="port">
                                                <b-form-input id="port"
                                                              v-model="smtpSettings.Port"
                                                              placeholder="Port bilgisini giriniz" />
                                            </b-form-group>
                                        </b-col>
                                        <b-col lg="6"
                                               md="6"
                                               sm="12">
                                            <b-form-group label="Kullanıcı adı / E-Posta"
                                                          label-for="username">
                                                <b-form-input id="username"
                                                              v-model="smtpSettings.Username"
                                                              placeholder="Kullanıcı adı / E-Posta bilgisini giriniz" />
                                            </b-form-group>
                                        </b-col>
                                        <b-col lg="6"
                                               md="6"
                                               sm="12">
                                            <b-form-group label="Şifre"
                                                          label-for="password">
                                                <b-form-input id="password"
                                                              v-model="smtpSettings.Password"
                                                              placeholder="Şifre bilgisini giriniz" />
                                            </b-form-group>
                                        </b-col>
                                        <b-col cols="12">
                                            <b-form-group>
                                                <b-form-checkbox v-model="smtpSettings.IsEnableSsl"
                                                                 name="check-button"
                                                                 switch
                                                                 inline>
                                                    SSL Kullanılsın mı?
                                                </b-form-checkbox>
                                            </b-form-group>
                                        </b-col>
                                    </b-row>
                                </b-card>
                            </b-tab>
                            <b-tab>
                                <template #title>
                                    <feather-icon icon="ShieldIcon"
                                                  size="18"
                                                  class="mr-50" />
                                    <span class="font-weight-bold">Google ReCaptcha</span>
                                </template>
                                <b-card id="card-email"
                                        header-tag="header">
                                    <template #header>
                                        <span class="font-weight-bold" style="font-size:18px;">Google ReCaptcha Ayarları</span>
                                    </template>
                                    <p>Bu ayarlar tamamen <b class="text-primary">Recaptcha 2</b> ile uyumludur. Yeni bir ReCaptcha hesabı oluşturmak için <a href="https://www.google.com/recaptcha/about/" target="_blank" title="Google Recaptcha">tıklayınız</a>.</p>
                                    <b-form-group label="Site Key"
                                                  label-for="SiteKey"
                                                  class="mt-1">
                                        <b-form-input id="SiteKey"
                                                      v-model="reCaptcha.SiteKey"
                                                      placeholder="Site Key bilgisini giriniz" />
                                    </b-form-group>
                                    <b-form-group label="Secret Key"
                                                  label-for="SecretKey">
                                        <b-form-input id="SecretKey"
                                                      v-model="reCaptcha.SecretKey"
                                                      placeholder="Secret Key bilgisini giriniz" />
                                    </b-form-group>
                                    </b-card>
                            </b-tab>
                        </b-tabs>
                    </b-col>
                </b-row>
            </b-overlay>
        </b-col>
    </b-row>
</template>

<script>
    import 'quill/dist/quill.snow.css'

    import { BRow, BCol, BBreadcrumb, BBreadcrumbItem, BButton, BOverlay, BTabs, BTab, BCard, BFormGroup, BFormInput, BFormCheckbox } from 'bootstrap-vue';
    import axios from 'axios';
    import ToastificationContent from '@core/components/toastification/ToastificationContent.vue';

    export default {
        components: {
            ToastificationContent,
            BRow,
            BCol,
            BBreadcrumb,
            BBreadcrumbItem,
            BButton,
            BOverlay,
            BTabs,
            BTab,
            BCard,
            BFormGroup,
            BFormInput,
            BFormCheckbox,
        },
        data() {
            return {
                overlayShow: true,
                smtpSettings: {
                    SenderEmail: '',
                    Server: '',
                    Port: '',
                    Username: '',
                    Password: '',
                    IsEnableSsl: false,
                },
                reCaptcha: {
                    SiteKey: '',
                    SecretKey: '',
                }
            }
        },
        methods: {
            getData() {
                axios.get('/admin/settings-form')
                    .then((response) => {
                        if (response.data.SmtpSettings != null && response.data.ReCaptcha != null) {
                            this.smtpSettings.SenderEmail = response.data.SmtpSettings.SenderEmail;
                            this.smtpSettings.Server = response.data.SmtpSettings.Server;
                            this.smtpSettings.Port = response.data.SmtpSettings.Port;
                            this.smtpSettings.Username = response.data.SmtpSettings.Username;
                            this.smtpSettings.Password = response.data.SmtpSettings.Password;
                            this.smtpSettings.IsEnableSsl = response.data.SmtpSettings.IsEnableSsl;

                            this.reCaptcha.SiteKey = response.data.ReCaptcha.SiteKey;
                            this.reCaptcha.SecretKey = response.data.ReCaptcha.SecretKey;

                            this.overlayShow = false;
                        }
                    })
                    .catch((error) => {
                        this.$toast({
                            component: ToastificationContent,
                            props: {
                                variant: 'danger',
                                title: 'Hata Oluştu!',
                                icon: 'AlertOctagonIcon',
                                text: 'Form ayarları yüklenirken hata oluştu. Lütfen tekrar deneyin.',
                            }
                        })
                    });
                
            },
            updateData() {
                axios.post('/admin/settings-form', {
                    SmtpSettings: this.smtpSettings,
                    ReCaptcha: this.reCaptcha
                })
                    .then((response) => {
                        if (response.data.SmtpSettings != null && response.data.ReCaptcha != null) {
                            this.$toast({
                                component: ToastificationContent,
                                props: {
                                    variant: 'success',
                                    title: 'Başarılı İşlem!',
                                    icon: 'CheckIcon',
                                    text: 'Form ayarları güncellendi.'
                                }
                            });
                            this.overlayShow = false;
                        }
                    })
                    .catch((error) => {
                        this.$toast({
                            component: ToastificationContent,
                            props: {
                                variant: 'danger',
                                title: 'Hata Oluştu!',
                                icon: 'AlertOctagonIcon',
                                text: 'Form ayarları yüklenirken hata oluştu. Lütfen tekrar deneyin.',
                            }
                        })
                    });
            }
        },
        mounted() {
            this.getData();
        }
    }
</script>

<style lang="scss">
    #tab-form-settings .card .card-header {
        padding: 8px 8px 8px 20px;
        border-bottom: 1px solid #ebe9f1;
        margin-bottom: 20px;
    }
</style>