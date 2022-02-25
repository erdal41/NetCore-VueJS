<template>
    <b-row>
        <modal-upload-edit :upload-id="uploadId"
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
                        <b-input-group size="sm">
                            <b-input-group-prepend is-text>
                                <feather-icon icon="SearchIcon" />
                            </b-input-group-prepend>
                            <b-form-input placeholder="Ara..."
                                          v-model="filterText" />
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
                <b-card-body>
                    <div v-if="isSpinnerShow == true"
                         class="text-center mt-2 mb-2">
                        <b-spinner variant="primary" />
                    </div>
                    <div v-else>
                        <b-list-group id="grid-images"
                                      horizontal="md">
                            <b-list-group-item v-for="upload in uploads" :key="upload.Id"
                                               @click="imageClick($event, upload.Id)"
                                               v-b-modal="multiSelect == false ? 'upload-modal' : ''">
                                <div class="media-file"
                                     :class="multiSelect === true && selectedImages.includes(upload.Id) ? 'checked-image' : ''">
                                    <b-form-checkbox v-if="multiSelect && selectedImages.includes(upload.Id)"
                                                     v-model="selectedImages"
                                                     name="checkbox"
                                                     :value="upload.Id"
                                                     class="custom-control-primary check-image">
                                    </b-form-checkbox>
                                    <b-img 
                                           :key="upload.Id"
                                           :src="upload.FileName == null ? null : require('@/assets/images/media/' + upload.FileName)"
                                           :alt="upload.AltText"
                                           class="select-image"
                                           :style="multiSelect === true && !selectedImages.includes(upload.Id) ? 'opacity:0.5' : ''" />
                                    <b-progress v-if="uploads.some(up => up.Id != upload.Id) && isImageProgress"
                                                animated
                                                :value="progressPercent"
                                                variant="primary"
                                                class="img-progress progress-bar-primary" />
                                </div>

                            </b-list-group-item>
                        </b-list-group>
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
        BBreadcrumb, BBreadcrumbItem, BSpinner, BFormFile, BListGroup, BListGroupItem, BProgress, BImg, BFormCheckbox, BButton, BCard, BCardBody, BCardTitle, BRow, BCol, BInputGroup, BFormInput, BInputGroupPrepend, VBTooltip, BLink
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
            BImg,
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
                        this.uploads.unshift({
                            Id: -1,
                            FileName: null,
                            AltText: null,
                        });
                        console.log(event.target.files)
                        console.log(event)
                        console.log('eeee')
                        console.log(response.data)
                        if (response.data.UploadDtos != null) {
                            response.data.UploadDtos.forEach(uploadDto => {                                
                                this.newFiles.push(uploadDto.Upload.Id);
                                
                                if (this.progressPercent === 100) {
                                    this.isImageProgress = false;
                                    //this.uploads.unshift(uploadDto.Upload);
                                    this.uploads.forEach(upload => {
                                        if (upload.Id === uploadDto.Upload.Id) {
                                            upload.FileName = uploadDto.Upload.FileName
                                        }
                                    });
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
            onFiltered(filteredItems) {
                // Trigger pagination to update the number of buttons/pages due to filtering
                this.totalRows = filteredItems.length
                this.currentPage = 1
            },
        },
        computed: {
            filteredData: function () {
                return this.terms
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
    #grid-images.list-group {
        flex-wrap: wrap;
        align-content: space-between;
    }

    #grid-images .list-group-item {
        border: 0 !important;
        padding: 0 13.8px 13.8px 0 !important; 
    }

    #grid-images .list-group-item:hover {
        background-color: transparent !important;
    }

    .img-progress {
        position: relative;
        top: 25%;
    }

    .media-file {
        max-width: 110px;
        width: 110px;
        max-height: 110px;
        height: 110px;
        padding: 3px;
        cursor: pointer;
        -webkit-box-shadow: inset 0px 0px 2px 0px rgba(0,0,0,0.75);
        -moz-box-shadow: inset 0px 0px 2px 0px rgba(0,0,0,0.75);
        box-shadow: inset 0px 0px 2px 0px rgba(0,0,0,0.75);
        border-radius: 5px !important;
    }

    .media-file:hover {
        -webkit-box-shadow: 0px 0px 2px 2px rgba(115,103,240,1);
        -moz-box-shadow: 0px 0px 2px 2px rgba(115,103,240,1);
        box-shadow: 0px 0px 2px 2px rgba(115,103,240,1);
    }

    .check-image {
        position: absolute !important;
        top: -10px;
        right: 0;
    }

    .select-image {
        max-width: 100%;
        max-height: 100%;
        position: relative;
        top: 50%;
        left: 50%;
        transform: translateX(-50%) translateY(-50%);
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