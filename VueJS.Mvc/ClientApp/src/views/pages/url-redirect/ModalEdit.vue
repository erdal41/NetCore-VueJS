<template>
    <div>
        <b-modal id="modal-edit"
                 cancel-variant="outline-secondary"
                 ok-title="Güncelle"
                 cancel-title="İptal"
                 centered
                 title="Yönlendirme Düzenle"
                 :no-close-on-backdrop="true"
                 @show="getData"
                 @hidden="getAllData"
                 @ok="validationForm">
            <validation-observer ref="simpleRules">
                <b-form>
                    <b-row>
                        <b-col cols="12">
                            <b-form-group label-for="oldurl"
                                          description="Yönlendirilecek linki giriniz.">
                                <validation-provider #default="{ errors }"
                                                     name="oldurl"
                                                     rules="required">
                                    <b-form-input id="oldurl"
                                                  v-model="urlRedirectUpdateDto.OldUrl"
                                                  :state="errors.length > 0 ? false:null"
                                                  type="text"
                                                  placeholder="Eski Url" />
                                    <small class="text-danger">{{ errors[0] }}</small>
                                </validation-provider>
                            </b-form-group>

                            <b-form-group label-for="newurl"
                                          description="Yönelecek olan linki giriniz.">
                                <validation-provider #default="{ errors }"
                                                     name="newurl"
                                                     rules="required">
                                    <b-form-input id="newurl"
                                                  v-model="urlRedirectUpdateDto.NewUrl"
                                                  :state="errors.length > 0 ? false:null"
                                                  type="text"
                                                  placeholder="Yeni Url" />
                                    <small class="text-danger">{{ errors[0] }}</small>
                                </validation-provider>
                            </b-form-group>

                            <b-form-textarea id="description"
                                             v-model="urlRedirectUpdateDto.Description"
                                             placeholder="Açıklama"
                                             rows="2" />
                        </b-col>
                    </b-row>
                </b-form>
            </validation-observer>
        </b-modal>
    </div>

</template>

<script>
    import { ValidationProvider, ValidationObserver, extend } from 'vee-validate'
    import { required } from '@validations'
    import {
        BRow, BCol, BForm, BFormGroup, BFormInput, BFormTextarea
    } from 'bootstrap-vue'
    //import { codeRowDetailsSupport } from './code'
    import axios from 'axios'
    import ToastificationContent from '@core/components/toastification/ToastificationContent.vue'

    extend('required', {
        ...required,
        message: 'Lütfen gerekli bilgileri yazınız.'
    });

    export default {
        components: {
            BRow,
            BCol,
            BForm,
            BFormGroup,
            BFormInput,
            BFormTextarea,
            ValidationProvider,
            ValidationObserver,
            ToastificationContent
        },
        props: {
            urlredirectId: {
                type: Number,
                required: true,
            },
        },
        data() {
            return {
                required,
                urlRedirectUpdateDto: {
                    Id: this.urlredirectId,
                    OldUrl: '',
                    NewUrl: '',
                    Description: '',
                },                
            }
        },
        methods: {
            getAllData() {
                this.$emit('getAllData');
            },
            getData() {
                console.log('Number');
                axios.get('/admin/urlredirect-edit', {
                    params: {
                        urlRedirectId: this.urlredirectId
                    }
                }).then((response) => {
                    console.log(response.data);
                    this.urlRedirectUpdateDto.OldUrl = response.data.UrlRedirectUpdateDto.Data.OldUrl;
                    this.urlRedirectUpdateDto.NewUrl = response.data.UrlRedirectUpdateDto.Data.NewUrl;
                    this.urlRedirectUpdateDto.Description = response.data.UrlRedirectUpdateDto.Data.Description;
                })
            },
            validationForm() {
                this.$refs.simpleRules.validate().then(success => {
                    if (success) {
                        if (!this.urlRedirectUpdateDto.OldUrl.includes('https://') && !this.urlRedirectUpdateDto.OldUrl.includes('http://')) {
                            this.$toast({
                                component: ToastificationContent,
                                props: {
                                    variant: 'warning',
                                    title: 'Uyarı!',
                                    icon: 'AlertTriangleIcon',
                                    text: 'Eski url, "https://" veya "http://" parametresi ile beraber tam linki içermerlidir. Örnek: https://www.orneksite.com/ornek-sayfa'
                                }
                            });
                        } else if (!this.urlRedirectUpdateDto.NewUrl.includes('https://') && !this.urlRedirectUpdateDto.NewUrl.includes('http://')) {
                            this.$toast({
                                component: ToastificationContent,
                                props: {
                                    variant: 'warning',
                                    title: 'Uyarı!',
                                    icon: 'AlertTriangleIcon',
                                    text: 'Yeni url, "https://" veya "http://" parametresi ile beraber tam linki içermerlidir. Örnek: https://www.orneksite.com/ornek-sayfa'
                                }
                            });
                        } else {
                            axios.post('/admin/urlredirect-edit',
                                {
                                    UrlRedirectUpdateDto: this.urlRedirectUpdateDto
                                })
                                .then((response) => {
                                    console.log(response.data);
                                    if (response.data.UrlRedirectDto.ResultStatus === 0) {
                                        this.$toast({
                                            component: ToastificationContent,
                                            props: {
                                                variant: 'success',
                                                title: 'Başarılı İşlem!',
                                                icon: 'CheckIcon',
                                                text: this.urlRedirectUpdateDto.OldUrl + " linki " + this.urlRedirectUpdateDto.NewUrl + " linkine yönlendirildi."
                                            }
                                        });
                                    }
                                    else {
                                        this.$toast({
                                            component: ToastificationContent,
                                            props: {
                                                variant: 'danger',
                                                title: 'Başarısız İşlem!',
                                                icon: 'AlertOctagonIcon',
                                                text: response.data.UrlRedirectDto.Message
                                            },
                                        });
                                    }
                                })
                                .catch((error) => {
                                    console.log(error);
                                    console.log(error.request);
                                    this.$toast({
                                        component: ToastificationContent,
                                        props: {
                                            variant: 'danger',
                                            title: 'Hata Oluştu!',
                                            icon: 'AlertOctagonIcon',
                                            text: 'Hata oluştu. Lütfen tekrar deneyiniz.',
                                        },
                                    })
                                });
                        }
                    }
                })
            },
        }
    }
</script>