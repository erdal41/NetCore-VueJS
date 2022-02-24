<template>
    <div>
        <b-modal id="upload-modal"
                 cancel-variant="outline-secondary"
                 ok-title="Güncelle"
                 cancel-title="İptal"
                 title="Medya Düzenle"
                 :no-close-on-backdrop="true"
                 @show="getData">
            <template #modal-header>
                <div class="content-header-left"
                     cols="12"
                     md="6">
                    <h3 class="modal-title float-left">
                        Medya Detayları
                    </h3>
                </div>
                <div class="content-header-right text-md-right d-md-block d-none"
                     md="6"
                     cols="12">
                    <b-button-group size="sm">
                        <b-button variant="flat-secondary"
                                  class="btn-icon rounded-circle">
                            <feather-icon icon="ChevronLeftIcon" />
                        </b-button>
                        <b-button variant="flat-secondary"
                                  class="btn-icon rounded-circle">
                            <feather-icon icon="ChevronRightIcon" />
                        </b-button>
                        <b-button variant="flat-secondary"
                                  class="btn-icon rounded-circle"
                                  @click="$bvModal.hide('upload-modal')">
                            <feather-icon icon="XIcon" />
                        </b-button>
                    </b-button-group>
                </div>
            </template>
            <div class="media-details container-fluid h-100 p-0">
                <b-row class="h-100">
                    <b-col cols="8">
                        <div class="image-view">
                            <b-img :src="uploadUpdateDto.FileName == null ? '' : require('@/assets/images/media/' + uploadUpdateDto.FileName)" fluid></b-img>
                        </div>
                    </b-col>
                    <b-col cols="4"
                           class="image-info">
                        <div style="font-size:11px;">
                            <span><strong>Yüklenen Tarih: </strong>{{createdDate}}</span><br />
                            <span><strong>Yükleyen: </strong>{{ uploadUpdateDto.User.UserName}}</span><br />
                            <span><strong>Dosya Adı: </strong>{{ uploadUpdateDto.FileName}}</span><br />
                            <span><strong>Dosya Türü: </strong>{{ uploadUpdateDto.ContentType}}</span><br />
                            <span><strong>Dosya Boyutu: </strong>{{sizeConvert}}</span><br />
                            <span><strong>Ölçüler: </strong>{{ uploadUpdateDto.Width }}x{{ uploadUpdateDto.Height }}px</span>
                        </div>
                        <hr />
                        <b-form-group label-cols="4"
                                      label-size="sm"
                                      label="Alternatif Metin"
                                      label-for="altText">
                            <b-form-input id="altText"
                                          v-model="uploadUpdateDto.AltText"
                                          type="text"
                                          size="sm"></b-form-input>
                        </b-form-group>
                        <b-form-group label-cols="4"
                                      label-size="sm"
                                      label="Başlık"
                                      label-for="title">
                            <b-form-input id="title"
                                          v-model="uploadUpdateDto.Title"
                                          type="text"
                                          size="sm"></b-form-input>
                        </b-form-group>
                        <b-form-group label-cols="4"
                                      label-size="sm"
                                      label="Alt Başlık"
                                      label-for="subTitle">
                            <b-form-input id="subTitle"
                                          v-model="uploadUpdateDto.SubTitle"
                                          type="text"
                                          size="sm"></b-form-input>
                        </b-form-group>
                        <b-form-group label-cols="4"
                                      label-size="sm"
                                      label="Açıklama"
                                      label-for="description">
                            <b-form-textarea id="description"
                                             v-model="uploadUpdateDto.Description"
                                             size="sm"></b-form-textarea>
                        </b-form-group>
                        <b-form-group label-cols="4"
                                      label-size="sm"
                                      label="Dosya Adresi"
                                      label-for="fileUrl">
                            <b-form-input id="fileUrl"
                                          v-model="altText"
                                          type="text"
                                          size="sm"
                                          readonly></b-form-input>
                        </b-form-group>
                    </b-col>
                </b-row>
            </div>
        </b-modal>
    </div>
</template>

<script>
    import {
        BRow, BCol, BImg, BFormGroup, BFormInput, BFormTextarea, BButtonGroup, BButton
    } from 'bootstrap-vue'
    //import { codeRowDetailsSupport } from './code'
    import axios from 'axios'
    import ToastificationContent from '@core/components/toastification/ToastificationContent.vue'
    import moment from 'moment'

    export default {
        components: {
            BRow,
            BCol,
            BImg,
            BFormGroup,
            BFormInput,
            BFormTextarea,
            ToastificationContent,
            BButtonGroup,
            BButton
        },
        props: {
            uploadId: {
                type: Number,
                required: true,
            },
        },
        data() {
            return {
                uploadUpdateDto: {
                    AltText: null,
                    ContentType: null,
                    CreatedDate: null,
                    Description: null,
                    FileName: null,
                    FileUrl: null,
                    Height: null,
                    Id: this.uploadId,
                    Size: null,
                    SubTitle: null,
                    Title: null,
                    User: [],
                    UserId: null,
                    Width: null,
                },
                createdDate: '',
                sizeConvert: '',
                altText: '',
            }
        },
        methods: {
            getData() {
                axios.get('/admin/upload-edit', {
                    params: {
                        uploadId: this.uploadId
                    }
                }).then((response) => {
                    console.log(response.data);
                    this.uploadUpdateDto.AltText = response.data.UploadUpdateDto.AltText;
                    this.uploadUpdateDto.ContentType = response.data.UploadUpdateDto.ContentType;
                    this.uploadUpdateDto.CreatedDate = response.data.UploadUpdateDto.CreatedDate;
                    this.uploadUpdateDto.Description = response.data.UploadUpdateDto.Description;
                    this.uploadUpdateDto.FileName = response.data.UploadUpdateDto.FileName;
                    this.uploadUpdateDto.FileUrl = response.data.UploadUpdateDto.FileUrl;
                    this.uploadUpdateDto.Height = response.data.UploadUpdateDto.Height;
                    this.uploadUpdateDto.SubTitle = response.data.UploadUpdateDto.SubTitle;
                    this.uploadUpdateDto.Title = response.data.UploadUpdateDto.Title;
                    this.uploadUpdateDto.User = response.data.UploadUpdateDto.User;
                    this.uploadUpdateDto.UserId = response.data.UploadUpdateDto.UserId;
                    this.uploadUpdateDto.Width = response.data.UploadUpdateDto.Width;
                    this.uploadUpdateDto.Size = response.data.UploadUpdateDto.Size;
                    console.log(location.origin);
                    var imgPath = require('@/assets/images/media/' + response.data.UploadUpdateDto.FileName)
                    this.altText = location.origin + imgPath;
                    this.createdDate = moment(new Date(response.data.UploadUpdateDto.CreatedDate)).format('DD.MM.YYYY');


                    var size = response.data.UploadUpdateDto.Size;
                    console.log(size)
                    if (size >= 1024) {
                        this.sizeConvert = (size / 1024).toFixed(0) + " KB";
                    } else if (size >= 1048576) {
                        this.sizeConvert = (size / 1048576).toFixed(0) + " MB";
                    }
                    else if (size > 1073741824) {
                        this.sizeConvert = (size / 1073741824).toFixed(0) + " GB";
                    }
                    else {
                        this.sizeConvert = size + " Bayt";
                    }
                    console.log(this.sizeConvert)
                })
            },
            rightGetData(id) {
                axios.get('/admin/upload-edit', {
                    params: {
                        uploadId: this.uploadId
                    }
                }).then((response) => {
                    console.log(response.data);
                    this.uploadUpdateDto.AltText = response.data.UploadUpdateDto.AltText;
                    this.uploadUpdateDto.ContentType = response.data.UploadUpdateDto.ContentType;
                    this.uploadUpdateDto.CreatedDate = response.data.UploadUpdateDto.CreatedDate;
                    this.uploadUpdateDto.Description = response.data.UploadUpdateDto.Description;
                    this.uploadUpdateDto.FileName = response.data.UploadUpdateDto.FileName;
                    this.uploadUpdateDto.FileUrl = response.data.UploadUpdateDto.FileUrl;
                    this.uploadUpdateDto.Height = response.data.UploadUpdateDto.Height;
                    this.uploadUpdateDto.SubTitle = response.data.UploadUpdateDto.SubTitle;
                    this.uploadUpdateDto.Title = response.data.UploadUpdateDto.Title;
                    this.uploadUpdateDto.User = response.data.UploadUpdateDto.User;
                    this.uploadUpdateDto.UserId = response.data.UploadUpdateDto.UserId;
                    this.uploadUpdateDto.Width = response.data.UploadUpdateDto.Width;
                    this.uploadUpdateDto.Size = response.data.UploadUpdateDto.Size;
                    console.log(location.origin);
                    var imgPath = require('@/assets/images/media/' + response.data.UploadUpdateDto.FileName)
                    this.altText = location.origin + imgPath;
                    this.createdDate = moment(new Date(response.data.UploadUpdateDto.CreatedDate)).format('DD.MM.YYYY');


                    var size = response.data.UploadUpdateDto.Size;
                    console.log(size)
                    if (size >= 1024) {
                        this.sizeConvert = (size / 1024).toFixed(0) + " KB";
                    } else if (size >= 1048576) {
                        this.sizeConvert = (size / 1048576).toFixed(0) + " MB";
                    }
                    else if (size > 1073741824) {
                        this.sizeConvert = (size / 1073741824).toFixed(0) + " GB";
                    }
                    else {
                        this.sizeConvert = size + " Bayt";
                    }
                    console.log(this.sizeConvert)
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
        },
        mounted() {
        }
    }
</script>

<style>
    #upload-modal .modal-dialog {
        max-width: 100%;
        height: 100vh;
        margin: 0;
        padding: 30px !important;
        display: flex;
    }

    #upload-modal .modal-header {
        padding: 0 0 0 10px;
    }

    #upload-modal .modal-body {
        border-top: 1px solid #dcdcde;
        padding: 0 14px 0 14px !important
    }

    .media-details {
        box-shadow: inset 0 4px 4px -4px rgb(0 0 0 / 10%);
    }

    .image-info {
        background-color: #f2f2f2;
        padding-top: 10px;
        border-left: 1px solid #dcdcde;
    }

    .image-view {
        text-align: center;
        margin-top: 10px;
        margin-bottom: 10px;
    }

        .image-view img {
            max-height: 743px !important;
        }
</style>