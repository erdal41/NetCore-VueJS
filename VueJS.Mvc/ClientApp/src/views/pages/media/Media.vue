<template>
    <b-row>
        <modal-upload-edit :upload-id="uploadId"
                           :filtered-data="filteredData"
                           @mediaGetAllData="mediaListDeleteData"></modal-upload-edit>
        <b-col class="content-header-left mb-2"
               cols="12"
               md="6">
            <b-row class="breadcrumbs-top">
                <b-col cols="12">
                    <h2 class="content-header-title float-left pr-1 mb-0">
                        Medya
                    </h2>
                    <div class="breadcrumb-wrapper">
                        <b-breadcrumb>
                            <b-breadcrumb-item to="/">
                                <feather-icon icon="HomeIcon"
                                              size="16"
                                              class="align-text-top" />
                            </b-breadcrumb-item>
                            <b-breadcrumb-item v-for="item in breadcrumbs"
                                               :key="item.text"
                                               :active="item.active"
                                               :to="item.to">
                                {{ item.text }}
                            </b-breadcrumb-item>
                        </b-breadcrumb>
                    </div>
                </b-col>
            </b-row>
        </b-col>
        <b-col class="content-header-right text-md-right d-md-block d-none mb-1"
               md="6"
               cols="12">
            <label class="btn btn-primary m-0">
                Yeni Ekle
                <b-form-file hidden="hidden"
                             plain
                             multiple
                             accept="image/jpeg, image/png, image/gif"
                             @change.prevent="addFiles" />
            </label>
            <b-button 
                   @click.prevent="isShowModal = true">asdsa</b-button>
        </b-col>
        <b-col cols="12">
            <b-card header-tag="header"
                    no-body>
                <template #header>
                    <div v-if="multiSelect === false"
                         class="float-left">
                        <b-button v-show="$can('create', 'Basepage')"
                                  v-b-tooltip.hover
                                  variant="outline-primary"
                                  size="sm"
                                  @click.prevent="multiSelect = true">Toplu Seçim</b-button>
                    </div>
                    <div v-if="multiSelect === true"
                         class="float-left">
                        <b-button v-show="$can('create', 'Basepage')"
                                  variant="primary"
                                  size="sm"
                                  :disabled="selectedImages.length <= 0"
                                  @click="multiDeleteData"> {{ selectedImagesCount }} Kalıcı Olarak Sil</b-button>
                        <b-button v-show="$can('create', 'Basepage')"
                                  variant="outline-primary"
                                  class=" ml-1"
                                  size="sm"
                                  @click.prevent="multiSelect = false; selectedImages = []; selectedImagesCount = ''">Vazgeç</b-button>
                    </div>
                    <div v-if="multiSelect === false"
                         class="ml-auto">
                        <b-input-group size="sm"
                                       class="input-group-merge"
                                       v-on:mouseover="isShowSearchTextClearButton = true"
                                       v-on:mouseleave="isShowSearchTextClearButton = false">
                            <b-input-group-prepend is-text>
                                <feather-icon icon="SearchIcon" />
                            </b-input-group-prepend>
                            <b-form-input placeholder="Ara..."
                                          v-model="filterText" />
                            <b-input-group-append is-text>
                                <feather-icon v-show="isShowSearchTextClearButton"
                                              icon="XIcon"
                                              v-b-tooltip.hover
                                              title="Temizle"
                                              class="cursor-pointer"
                                              @click="filterText = ''"/>
                            </b-input-group-append>
                        </b-input-group>
                    </div>
                    <div v-if="multiSelect === false"
                         class="ml-auto">
                        <b-button v-b-tooltip.hover
                                  title="Dosyaları Yenile"
                                  v-ripple.400="'rgba(113, 102, 240, 0.15)'"
                                  variant="fade-secondary"
                                  class="btn-icon mr-1"
                                  size="sm"
                                  @click="getAllData">
                            <feather-icon icon="RotateCcwIcon" />
                        </b-button>
                    </div>
                </template>
                <b-card-body class="p-0">
                    <div v-if="isSpinnerShow == true"
                         class="text-center mt-2 mb-2">
                        <b-spinner variant="primary" />
                    </div>
                    <div v-else>
                        <ul class="gallery">
                            <li v-for="upload in filteredData" :key="upload.Id"
                                class="gallery-item"
                                               @click="imageClick($event, upload.Id)"
                                               v-b-modal="multiSelect == false ? 'upload-modal' : ''">
                                <div class="media"
                                     :class="multiSelect === true && selectedImages.includes(upload.Id) ? 'checked-image' : ''">
                                    <b-form-checkbox v-if="multiSelect && selectedImages.includes(upload.Id)"
                                                     v-model="selectedImages"
                                                     name="checkbox"
                                                     :value="upload.Id"
                                                     class="custom-control-primary check-image">
                                    </b-form-checkbox>
                                    <div class="thumbnail">
                                        <div class="centered">
                                            <b-img-lazy :key="upload.Id"
                                                        :src="upload.FileName == null ? null : require('@/assets/images/media/' + upload.FileName)"
                                                        :alt="upload.AltText"
                                                        :style="multiSelect === true && !selectedImages.includes(upload.Id) ? 'opacity:0.5' : ''" />
                                            <b-progress v-if="uploads.includes(upload.Id) && isImageProgress"
                                                        animated
                                                        :value="progressPercent"
                                                        variant="primary"
                                                        class="img-progress progress-bar-primary" />
                                        </div>
                                    </div>
                                </div>
                            </li>
                        </ul>
                    </div>
                </b-card-body>

                <div v-show="uploads.length <= 0"
                     class="text-center mt-1">{{ dataNullMessage  }}</div>
                <b-card-body>
                    <div class="d-flex justify-content-between flex-wrap">
                        <label> Dosya Sayısı: {{ uploads.length }}</label>
                    </div>
                </b-card-body>
            </b-card>
        </b-col>
    </b-row>
</template>

<script>
    import {
        BBreadcrumb, BBreadcrumbItem, BSpinner, BFormFile, BListGroup, BListGroupItem, BProgress, BImgLazy, BFormCheckbox, BButton, BCard, BCardBody, BCardTitle, BRow, BCol, BInputGroup, BFormInput, BInputGroupPrepend, BInputGroupAppend, VBTooltip, BLink
    } from 'bootstrap-vue'
    import ModalUploadEdit from './ModalUploadEdit.vue';
    import axios from 'axios'
    import ToastificationContent from '@core/components/toastification/ToastificationContent.vue'
    import Ripple from 'vue-ripple-directive'

    export default {
        components: {
            ModalUploadEdit,
            BBreadcrumb,
            BBreadcrumbItem,
            BSpinner,
            BFormFile,
            BListGroup,
            BListGroupItem,
            BProgress,
            BImgLazy,
            BCardTitle,
            BButton,
            BFormCheckbox,
            BCard,
            BRow,
            BCol,
            BCardBody,
            BInputGroup,
            BFormInput,
            BInputGroupPrepend,
            BInputGroupAppend,
            ToastificationContent,
            BLink
        },
        directives: {
            'b-tooltip': VBTooltip,
            Ripple,
        },
        data() {
            return {
                breadcrumbs: [
                    {
                        text: 'Medya',
                        active: true,
                    }
                ],
                isShowModal: false,
                uploadId: 0,
                isImageProgress: false,
                isSpinnerShow: true,
                progressPercent: 0,
                isShowSearchTextClearButton: false,
                filterText: '',
                filterOnData: [],
                uploads: [],
                shortedData: [],
                newFiles: [],
                dataNullMessage: '',
                selected: '',
                selectedValue: null,
                isHiddenMultiDeleteButton: false,
                isHiddenRowActions: false,
                name: "",
                checkedRows: [],
                checkedRowsCount: 0,
                selectedImages: [],
                selectedImagesCount: '',
                multiSelect: false,
                termAddDto: {
                    Name: "",
                    Slug: "",
                    ParentId: null,
                    Description: '',
                    TermType: 'tag'
                },
            }
        },
        methods: {
            addFiles: function (event) {
                if (event.target.files.length > 0) {
                    let formData = new FormData();
                    event.target.files.forEach(file => {
                        formData.append('files', file);
                    });
                    axios.post('/admin/upload-new', formData, {
                        headers: {
                            'Content-Type': 'multipart/form-data'
                        },
                        onUploadProgress: (uploadEvent) => {
                            this.isImageProgress = true;
                            this.progressPercent = uploadEvent.loaded / uploadEvent.total * 100;                            
                        }
                    }).then((response) => {                        
                        console.log(event.target.files)
                        console.log(event)
                        console.log('eeee')
                        console.log(response.data)
                        if (response.data.UploadDtos != null) {
                            response.data.UploadDtos.forEach(uploadDto => {                                
                                this.newFiles.push(uploadDto.Upload.Id);
                                this.uploads.unshift({
                                    Id: uploadDto.Upload.Id,
                                    FileName: null,
                                    AltText: null,
                                });
                                if (this.progressPercent === 100) {
                                    this.isImageProgress = false;
                                    //this.uploads.unshift(uploadDto.Upload);
                                    
                                    setTimeout(() => {
                                        this.uploads.forEach(upload => {

                                            if (upload.Id === uploadDto.Upload.Id) {
                                                upload.FileName = uploadDto.Upload.FileName
                                            }
                                        });
                                    }, 5000);
                                }
                            });


                        }
                    }).catch((error) => {
                        console.log(error)
                        console.log(error.request)
                    });

                }
            },
            asd() {
                this.isImageProgress = true;
                this.uploads.unshift({
                    FileName: null,
                    AltText: null,
                });
                this.uploadId = 1;
                console.log('ss');
                let element = this.$refs.modal.$el
                $(element).modal('show')
            },
            imageClick: function (event, id) {
                if (this.multiSelect === true) {
                    var isSelectImage = this.selectedImages.some(uploadId => uploadId === id);
                    if (isSelectImage) {
                        this.selectedImages = this.selectedImages.filter(uploadId => uploadId !== id);
                    } else {
                        this.selectedImages.push(id);
                    }

                    if (this.selectedImages.length > 0) {
                        this.selectedImagesCount = `( ${this.selectedImages.length} )`;
                    }
                    else {
                        this.selectedImagesCount = '';
                    }
                }
                else {
                    this.isShowModal = true;
                    this.uploadId = id;
                }
            },
            getAllData() {
                this.isSpinnerShow = true;
                axios.get('/admin/upload-alluploads')
                    .then((response) => {
                        console.log(response.data)
                        if (response.data.ResultStatus === 0) {
                            this.uploads = response.data.Data.Uploads;
                            this.isSpinnerShow = false;
                            this.selectedImages = [];
                            this.filterText = "";
                        }
                        else {
                            this.$toast({
                                component: ToastificationContent,
                                props: {
                                    variant: 'danger',
                                    title: 'Hata Oluştu!',
                                    icon: 'AlertOctagonIcon',
                                    text: 'Dosyalar listelenirken hata oluştu. ',
                                }
                            })
                        }
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
                                text: 'Hata oluştu. Lütfen tekrar deneyiniz.',
                            }
                        })
                    });
            },
            mediaListDeleteData(id, isDeleted) {
                if (isDeleted === true) {
                    this.uploads = this.uploads.filter(upload => upload.Id != id);
                }
            },
            multiDeleteData() {
                this.$swal({
                    title: 'Toplu olarak silmek istediğinizden emin misiniz?',
                    text: this.selectedImagesCount + " adet dosya kalıcı olarak silinecektir?",
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonText: 'Evet',
                    cancelButtonText: 'Hayır',
                    customClass: {
                        confirmButton: 'btn btn-primary',
                        cancelButton: 'btn btn-outline-danger ml-1',
                    },
                    buttonsStyling: false,
                }).then(result => {
                    if (result.value) {
                        this.selectedImages.forEach((id, index) => {
                            axios.post('/admin/upload-delete?uploadId=' + id)
                                .then((response) => {
                                    if (response.data.ResultStatus === 0) {
                                        this.selectedImagesCount = "";
                                        this.multiSelect = false;
                                        this.getAllData();
                                    }
                                });
                        });
                    }
                })
            },
            filterByName: function (data) {
                if (this.filterText.length === 0) {
                    return true;
                }

                return (data.FileName.toLowerCase().indexOf(this.filterText.toLowerCase()) > -1);
            },
        },
        computed: {
            filteredData: function () {
                return this.uploads
                    .filter(this.filterByName);
            }
        },
        mounted() {
            this.getAllData();
            this.totalRows = this.uploads.length;
        }
    }
</script>

<style lang="scss">

    .gallery {
        padding: 2px;
        right: 0;
        margin-right: 0;
        display: grid;
        grid-template-columns: repeat(auto-fill, minmax(138px, 1fr));
    }

    .gallery-item {
        padding: 8px;      
        list-style: none;        
    }

    .media {
        position: relative;
        box-shadow: inset 0 0 15px rgb(0 0 0 / 10%), inset 0 0 0 1px rgb(0 0 0 / 5%);
        background: #f0f0f1;
        cursor: pointer;
    }

    .media:before {
        content: "";
        display: block;
        padding-top: 100%;
    }

    .thumbnail {
        overflow: hidden;
        position: absolute;
        top: 0;
        right: 0;
        bottom: 0;
        left: 0;
        opacity: 1;
        transition: opacity .1s;
    }

    .thumbnail:hover {
        -webkit-box-shadow: 0px 0px 2px 2px rgba(115,103,240,1);
        -moz-box-shadow: 0px 0px 2px 2px rgba(115,103,240,1);
        box-shadow: 0px 0px 2px 2px rgba(115,103,240,1);
    }

    .thumbnail:after {
        content: "";
        display: block;
        position: absolute;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        box-shadow: inset 0 0 0 1px rgb(0 0 0 / 10%);
        overflow: hidden;
    }

    .thumbnail .centered {
        position: absolute;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        transform: translate(50%,50%);
    }

    .thumbnail img {
        transform: translate(-50%,-50%);
        max-height: 100%;
    }


    .img-progress {
        position: relative;
        top: 25%;
    }    

    .check-image {
        position: absolute !important;
        top: -11px;
        right: -16px;
    }

    .checked-image {
        -webkit-box-shadow: 0px 0px 2px 2px rgba(115,103,240,1);
        -moz-box-shadow: 0px 0px 2px 2px rgba(115,103,240,1);
        box-shadow: 0px 0px 2px 2px rgba(115,103,240,1);
    }

    .checked-image .custom-control-label::after {
        -webkit-box-shadow: 0px 0px 2px 2px rgba(115,103,240,1);
        -moz-box-shadow: 0px 0px 2px 2px rgba(115,103,240,1);
        box-shadow: 0px 0px 2px 2px rgba(115,103,240,1);
        border-radius: 4px;
    }   

    </style>