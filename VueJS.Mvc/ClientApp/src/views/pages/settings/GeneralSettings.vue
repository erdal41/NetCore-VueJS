﻿<template>
    <b-row class="content-header">
        <modal-media 
                     @changeImage="imageChange"></modal-media>
        <b-col class="content-header-left mb-2"
               cols="12"
               md="8">
            <h2 class="content-header-title float-left pr-1 mb-0">
                Genel Ayarlar
            </h2>
            <div class="breadcrumb-wrapper">
                <b-breadcrumb>
                    <b-breadcrumb-item to="/">
                        <feather-icon icon="HomeIcon"
                                      size="16"
                                      class="align-text-top" />
                    </b-breadcrumb-item>
                    <b-breadcrumb-item active>Genel Ayarları Düzenle</b-breadcrumb-item>
                </b-breadcrumb>
            </div>
        </b-col>
        <b-col class="content-header-right text-md-right d-md-block d-none mb-1"
               md="4"
               cols="12">
            <b-spinner v-if="buttonDisabled"
                       variant="secondary"
                       class="align-middle mr-1" />
            <b-button v-if="$can('update', 'Seo')"
                      :disabled="buttonDisabled"
                      :variant="updateButtonVariant"
                      type="button"
                      @click.prevent="updateData">
                Güncelle
            </b-button>
        </b-col>
        <b-col cols="12">
            <b-tabs id="tab-general-settings"
                    vertical
                    content-class="col-12 col-md-9 mt-1 mt-md-0"
                    pills
                    nav-wrapper-class="col-md-3 col-12"
                    nav-class="nav-left">

                <!-- general tab -->
                <b-tab active>

                    <!-- title -->
                    <template #title>
                        <feather-icon icon="CpuIcon"
                                      size="18"
                                      class="mr-50" />
                        <span class="font-weight-bold">Genel</span>
                    </template>


                </b-tab>
                <!--/ general tab -->
                <b-tab>

                    <!-- title -->
                    <template #title>
                        <feather-icon icon="ImageIcon"
                                      size="18"
                                      class="mr-50" />
                        <span class="font-weight-bold">Logo - İkon</span>
                    </template>
                    <b-card id="card-logo"
                            header-tag="header">
                        <template #header>
                            <span class="font-weight-bold" style="font-size:18px;">Logo Ayarları</span>
                        </template>
                        <b-row>
                            <b-col lg="6"
                                   md="6">
                                <span>Siteniz için logo seçiniz.</span>
                                <div class="image-thumb mt-1">
                                    <b-img
                                           v-bind:src="logoImage.fileName == null ? noImage : require('@/assets/images/media/' + logoImage.fileName)"
                                           :alt="logoImage.altText"
                                           rounded />
                                    <b-button 
                                              v-ripple.400="'rgba(186, 191, 199, 0.15)'"
                                              variant="relief-primary"
                                              size="sm"
                                              class="btn-icon rounded-circle selected-image select-logo-image"
                                              v-b-modal.modal-media
                                              @click="selectImage">
                                        <feather-icon icon="Edit2Icon"
                                                      class="select-logo-image"
                                                      size="11" />

                                    </b-button>
                                    <b-button v-if="logoImage.id != null"
                                              v-ripple.400="'rgba(186, 191, 199, 0.15)'"
                                              variant="relief-secondary"
                                              size="sm"
                                              class="btn-icon rounded-circle removed-image remove-logo-image"
                                              @click="removeImage($event)">
                                        <feather-icon icon="XIcon"
                                                      class="remove-logo-image"
                                                      size="11" />
                                    </b-button>
                                    <b-form-input type="text"
                                                  hidden
                                                  v-model="logoImage.id"></b-form-input>
                                </div>
                            </b-col>
                            <b-col lg="6"
                                   md="6">
                                <span>Siteniz için mobil logo seçiniz.</span>
                                <div class="image-thumb mt-1">
                                    <b-img
                                           v-bind:src="mobileLogoImage.fileName == null ? noImage : require('@/assets/images/media/' + mobileLogoImage.fileName)"
                                           :alt="mobileLogoImage.altText"
                                           rounded />
                                    <b-button 
                                              v-ripple.400="'rgba(186, 191, 199, 0.15)'"
                                              variant="relief-primary"
                                              size="sm"
                                              class="btn-icon rounded-circle selected-image select-mobilelogo-image"
                                              v-b-modal.modal-media
                                              @click="selectImage">
                                        <feather-icon icon="Edit2Icon"
                                                      class="select-mobilelogo-image"
                                                      size="11" />
                                    </b-button>
                                    <b-button v-if="mobileLogoImage.id != null"
                                              v-ripple.400="'rgba(186, 191, 199, 0.15)'"
                                              variant="relief-secondary"
                                              size="sm"
                                              class="btn-icon rounded-circle removed-image remove-mobilelogo-image" 
                                              @click="removeImage">
                                        <feather-icon icon="XIcon"
                                                      class="remove-mobilelogo-image"
                                                      size="11" />
                                    </b-button>
                                    <b-form-input type="text"
                                                  hidden
                                                  v-model="mobileLogoImage.id"></b-form-input>
                                </div>
                            </b-col>
                            <b-col cols="12" class="mt-3">
                                <b-form-group>
                                    <b-form-checkbox v-model="generalSettingUpdateDto.IsUseLogoAdminPanel"
                                                     name="check-button"
                                                     switch
                                                     inline>
                                        Logo yönetim panelinde kullanılsın mı?
                                    </b-form-checkbox>
                                </b-form-group>
                            </b-col>
                        </b-row>
                    </b-card>
                    <b-card id="card-icon"
                            header-tag="header">
                        <template #header>
                            <span class="font-weight-bold" style="font-size:18px;"> İkon Ayarları</span>
                        </template>
                        <b-row>
                            <b-col lg="6"
                                   md="6">
                                <span>Siteniz için ikon seçiniz.</span>
                                <div class="image-thumb-icon mt-1">
                                    <b-img 
                                           v-bind:src="iconImage.fileName == null ? noImage : require('@/assets/images/media/' + iconImage.fileName)"
                                           :alt="iconImage.altText"
                                           rounded />
                                    <b-button 
                                              v-ripple.400="'rgba(186, 191, 199, 0.15)'"
                                              variant="relief-primary"
                                              size="sm"
                                              class="btn-icon rounded-circle selected-image select-icon-image"
                                              v-b-modal.modal-media
                                              @click.prevent="selectImage">
                                        <feather-icon icon="Edit2Icon"
                                                      class="select-icon-image"
                                                      size="11" />
                                    </b-button>
                                    <b-button v-if="iconImage.id != null"
                                              v-ripple.400="'rgba(186, 191, 199, 0.15)'"
                                              variant="relief-secondary"
                                              size="sm"
                                              class="btn-icon rounded-circle removed-image remove-icon-image"
                                              @click="removeImage">
                                        <feather-icon icon="XIcon"
                                                      class="remove-icon-image"
                                                      size="11" />
                                    </b-button>
                                    <b-form-input type="text"
                                                  hidden
                                                  v-model="iconImage.id"></b-form-input>
                                </div>
                            </b-col>
                            <b-col cols="12" class="mt-3">
                                <b-form-group>
                                    <b-form-checkbox v-model="generalSettingUpdateDto.IsUseIconAdminPanel"
                                                     name="check-button"
                                                     switch
                                                     inline>
                                        İkon yönetim panelinde kullanılsın mı?
                                    </b-form-checkbox>
                                </b-form-group>
                            </b-col>
                        </b-row>
                    </b-card>
                </b-tab>
                <!--/ change password tab -->
                <!-- info -->
                <b-tab>

                    <!-- title -->
                    <template #title>
                        <feather-icon icon="BookOpenIcon"
                                      size="18"
                                      class="mr-50" />
                        <span class="font-weight-bold">Blog</span>
                    </template>
                    <b-card id="card-blog"
                            header-tag="header">
                        <template #header>
                            <span class="font-weight-bold" style="font-size:18px;">Blog Ayarları</span>
                        </template>
                        <b-row>
                            <b-col cols="12">
                                <b-form-group>
                                    <b-form-checkbox v-model="generalSettingUpdateDto.IsActiveArticleComments"
                                                     name="check-button"
                                                     switch
                                                     inline>
                                        Tüm yazılarda yorum özelliği aktif olsun mu?
                                    </b-form-checkbox>
                                </b-form-group>
                                <b-form-group>
                                    <b-form-checkbox v-model="generalSettingUpdateDto.IsShowArticleAuthor"
                                                     name="check-button"
                                                     switch
                                                     inline>
                                        Tüm yazılarda yayın tarihi görüntülensin mi?
                                    </b-form-checkbox>
                                </b-form-group>
                                <b-form-group>
                                    <b-form-checkbox v-model="generalSettingUpdateDto.IsShowArticleDate"
                                                     name="check-button"
                                                     switch
                                                     inline>
                                        Tüm yazılarda yazar adı görüntülensin mi?
                                    </b-form-checkbox>
                                </b-form-group>
                                <b-form-group>
                                    <b-form-checkbox v-model="generalSettingUpdateDto.IsShowArticleCommentCount"
                                                     name="check-button"
                                                     switch
                                                     inline>
                                        Tüm yazılarda yorum sayısı görüntülensin mi?
                                    </b-form-checkbox>
                                </b-form-group>
                                <b-form-group>
                                    <b-form-checkbox v-model="generalSettingUpdateDto.IsAdminCommentApprove"
                                                     name="check-button"
                                                     switch
                                                     inline>
                                        Yönetici yorumu için onay istenilsin mi?
                                    </b-form-checkbox>
                                </b-form-group>
                            </b-col>
                        </b-row>
                    </b-card>
                </b-tab>

                <!-- social links -->
                <b-tab>

                    <!-- title -->
                    <template #title>
                        <feather-icon icon="CodeIcon"
                                      size="18"
                                      class="mr-50" />
                        <span class="font-weight-bold">Özel CSS</span>
                    </template>

                    
</b-tab>

                <!-- roles -->
                <b-tab>

                    <!-- title -->
                    <template #title>
                        <feather-icon icon="CodeIcon"
                                      size="18"
                                      class="mr-50" />
                        <span class="font-weight-bold">Özel JS</span>
                    </template>

                    </b-tab>
            </b-tabs>
        </b-col>
    </b-row>
</template>

<script>
    import ModalMedia from '../media/ModalMedia.vue';
    import CodeEditor from 'simple-code-editor';
    import { BRow, BCol, BBreadcrumb, BBreadcrumbItem, BButton, BTabs, BTab, BCard, BSpinner, BImg, BFormGroup, BFormInput, BFormCheckbox } from 'bootstrap-vue';
    import axios from 'axios';
    import ToastificationContent from '@core/components/toastification/ToastificationContent.vue';    
    import Ripple from 'vue-ripple-directive'

    export default {
        components: {
            ModalMedia,
            CodeEditor,
            BRow,
            BCol,
            BBreadcrumb,
            BBreadcrumbItem,
            BButton,
            BTabs,
            BTab,
            BCard,
            BSpinner,
            BImg,
            BFormGroup,
            BFormInput,
            BFormCheckbox,
        },
        directives: {
            Ripple,
        },
        data() {
            return {
                buttonDisabled: false,
                updateButtonVariant: 'primary',
                noImage: require('@/assets/images/default/default-post-image.jpg'),
                isLogoImageChoose: false,
                isMobileLogoImageChoose: false,
                isIconImageChoose: false,
                logoImage: {
                    id: null,
                    fileName: null,
                    altText: null,
                },
                mobileLogoImage: {
                    id: null,
                    fileName: null,
                    altText: null,
                },
                iconImage: {
                    id: null,
                    fileName: null,
                    altText: null,
                },
                generalSettingUpdateDto: {
                    Id: 1,
                    LogoId: '',
                    MobileLogoId: '',
                    IconId: '',
                    IsUseLogoAdminPanel: false,
                    IsUseIconAdminPanel: false,
                    IsActiveArticleComments: false,
                    IsShowArticleDate: false,
                    IsShowArticleAuthor: false,
                    IsShowArticleCommentCount: false,
                    IsAdminCommentApprove: false,
                },
                JSCode: '',
                CSSCode: '',
            }
        },
        methods: {
            getData() {
                this.showOverlay = true;
                axios.get('/admin/settings-general')
                    .then((response) => {
                        console.log(response.data)
                        if (response.data.GeneralSettingUpdateDto.ResultStatus === 0) {
                            this.generalSettingUpdateDto.IsUseLogoAdminPanel = response.data.GeneralSettingUpdateDto.Data.IsUseLogoAdminPanel;
                            this.generalSettingUpdateDto.IsUseIconAdminPanel = response.data.GeneralSettingUpdateDto.Data.IsUseIconAdminPanel;
                            this.generalSettingUpdateDto.IsActiveArticleComments = response.data.GeneralSettingUpdateDto.Data.IsActiveArticleComments;
                            this.generalSettingUpdateDto.IsShowArticleDate = response.data.GeneralSettingUpdateDto.Data.IsShowArticleDate;
                            this.generalSettingUpdateDto.IsShowArticleAuthor = response.data.GeneralSettingUpdateDto.Data.IsShowArticleAuthor;
                            this.generalSettingUpdateDto.IsShowArticleCommentCount = response.data.GeneralSettingUpdateDto.Data.IsShowArticleCommentCount;
                            this.generalSettingUpdateDto.IsAdminCommentApprove = response.data.GeneralSettingUpdateDto.Data.IsAdminCommentApprove;

                            if (response.data.GeneralSettingUpdateDto.Data.Logo != null) {
                                this.logoImage.id = response.data.GeneralSettingUpdateDto.Data.Logo.Id
                                this.logoImage.fileName = response.data.GeneralSettingUpdateDto.Data.Logo.FileName
                                this.logoImage.altText = response.data.GeneralSettingUpdateDto.Data.Logo.AltText
                            }

                            if (response.data.GeneralSettingUpdateDto.Data.MobileLogo != null) {
                                this.mobileLogoImage.id = response.data.GeneralSettingUpdateDto.Data.MobileLogo.Id
                                this.mobileLogoImage.fileName = response.data.GeneralSettingUpdateDto.Data.MobileLogo.FileName
                                this.mobileLogoImage.altText = response.data.GeneralSettingUpdateDto.Data.MobileLogo.AltText
                            }

                            if (response.data.GeneralSettingUpdateDto.Data.Icon != null) {
                                this.iconImage.id = response.data.GeneralSettingUpdateDto.Data.Icon.Id
                                this.iconImage.fileName = response.data.GeneralSettingUpdateDto.Data.Icon.FileName
                                this.iconImage.altText = response.data.GeneralSettingUpdateDto.Data.Icon.AltText
                            }
                        }
                        this.showOverlay = false;
                    })
                    .catch((error) => {
                        console.log(error)
                        console.log(error.request)
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
            imageChange(id, name, altText) {
                if (this.isLogoImageChoose == true) {
                    this.logoImage.id = id;
                    this.logoImage.fileName = name;
                    this.logoImage.altText = altText;
                } else if (this.isMobileLogoImageChoose == true) {
                    this.mobileLogoImage.id = id;
                    this.mobileLogoImage.fileName = name;
                    this.mobileLogoImage.altText = altText;
                } else if (this.isIconImageChoose == true) {
                    this.iconImage.id = id;
                    this.iconImage.fileName = name;
                    this.iconImage.altText = altText;
                }
                this.isLogoImageChoose = false;
                this.isMobileLogoImageChoose = false;
                this.isIconImageChoose = false;
                
            },
            selectImage(e) {
                console.log(e.target._prevClass)
                if (e.target._prevClass.includes('select-logo-image')) {
                    this.isLogoImageChoose = true;
                } else if (e.target._prevClass.includes('select-mobilelogo-image')) {
                    this.isMobileLogoImageChoose = true;
                } else if (e.target._prevClass.includes('select-icon-image')) {
                    this.isIconImageChoose = true;
                }
            },
            removeImage: function (e) {
                if (e.target._prevClass.includes('remove-logo-image')) {
                    this.logoImage.id = null;
                    this.logoImage.fileName = null;
                    this.logoImage.altText = null;
                } else if (e.target._prevClass.includes('remove-mobilelogo-image')) {
                    this.mobileLogoImage.id = null;
                    this.mobileLogoImage.fileName = null;
                    this.mobileLogoImage.altText = null;
                } else if (e.target._prevClass.includes('remove-icon-image')) {
                    this.iconImage.id = null;
                    this.iconImage.fileName = null;
                    this.iconImage.altText = null;
                }
            },
            updateData() {
                this.buttonDisabled = true;
                this.updateButtonVariant = 'outline-secondary';
                this.generalSettingUpdateDto.LogoId = this.logoImage.id;
                this.generalSettingUpdateDto.MobileLogoId = this.mobileLogoImage.id;
                this.generalSettingUpdateDto.IconId = this.iconImage.id;
                axios.post('/admin/settings-general', {
                    GeneralSettingUpdateDto: this.generalSettingUpdateDto
                })
                    .then((response) => {
                        console.log(response.data)
                        if (response.data.GeneralSettingDto.ResultStatus === 0) {
                            this.$toast({
                                component: ToastificationContent,
                                props: {
                                    variant: 'success',
                                    title: 'Başarılı İşlem!',
                                    icon: 'CheckIcon',
                                    text: response.data.GeneralSettingDto.Message
                                }
                            });
                        }
                        this.buttonDisabled = false;
                        this.updateButtonVariant = 'primary';
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
    #tab-general-settings .card .card-header {
        padding: 8px 8px 8px 20px;
        border-bottom: 1px solid #ebe9f1;
        margin-bottom: 20px;
    }

    .image-thumb {
        width: 250px;
        height: 70px;
        -webkit-box-shadow: 0px 0px 3px 0px rgba(196,196,196,1);
        -moz-box-shadow: 0px 0px 3px 0px rgba(196,196,196,1);
        box-shadow: 0px 0px 3px 0px rgba(196,196,196,1);
        position: relative;
        border-radius: 5px;
    }

    .image-thumb-icon {
        width: 60px;
        height: 60px;
        -webkit-box-shadow: 0px 0px 3px 0px rgba(196,196,196,1);
        -moz-box-shadow: 0px 0px 3px 0px rgba(196,196,196,1);
        box-shadow: 0px 0px 3px 0px rgba(196,196,196,1);
        position: relative;
        border-radius: 5px;
    }

    .image-thumb img, .image-thumb-icon img {
        max-height: 100%;
        max-width: 100%;
        position: absolute;
        top: 0;
        bottom: 0;
        left: 0;
        right: 0;
        margin: auto;
        padding: 5px;
    }

    .selected-image {
        position: absolute !important;
        top: -12px;
        right: -12px;
        padding: 5px !important;
    }

    .removed-image {
        position: absolute !important;
        bottom: -12px;
        right: -12px;
        padding: 5px !important;
    }
</style>