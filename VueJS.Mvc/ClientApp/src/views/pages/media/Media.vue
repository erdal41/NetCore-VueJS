<template>
    <b-row>
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
                             accept="image/jpeg, image/png, image/gif" />
            </label>
            <label 
                   @click="asd">asdsa</label>
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
                                  :disabled="selectedImages.length <= 0">Kalıcı Olarak Sil</b-button>
                        <b-button v-show="$can('create', 'Basepage')"
                                  variant="outline-primary"
                                  class=" ml-1"
                                  size="sm"
                                  @click.prevent="multiSelect = false; selectedImages = []">Vazgeç</b-button>
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
                                  @click="getAllData()">
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
                        <b-list-group horizontal="md">
                            <b-list-group-item v-for="upload in uploads" :key="upload.Id"
                                               class="image-list"
                                               :class="multiSelect === true && selectedImages.includes(upload.Id) ? 'checked-image' : ''"
                                               @click="imageClick(upload.Id)">
                                <b-form-checkbox v-if="multiSelect && selectedImages.includes(upload.Id)"
                                                 v-model="selectedImages"
                                                 name="checkbox"
                                                 :value="upload.Id"
                                                 class="custom-control-primary check-image">
                                </b-form-checkbox>
                                <b-img rounded
                                       :key="upload.Id"
                                       :src="upload.FileName == null ? '' : require('@/assets/images/media/' + upload.FileName)"
                                       :alt="upload.AltText"
                                       class="d-inline-block select-image"
                                       :style="multiSelect === true && !selectedImages.includes(upload.Id) ? 'opacity:0.5' : ''" />
                                <b-progress                                             animated
                                            value="50"
                                            variant="primary"
                                            class="img-progress progress-bar-primary" />
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
    //import { codeRowDetailsSupport } from './code'
    import axios from 'axios'
    import ToastificationContent from '@core/components/toastification/ToastificationContent.vue'
    import Ripple from 'vue-ripple-directive'

    export default {
        components: {
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
                isShowProgress: false,
                isSpinnerShow: true,
                filterText: '',
                filterOnData: [],
                uploads: [],
                dataNullMessage: '',
                selected: '',
                selectedValue: null,
                isHiddenMultiDeleteButton: false,
                isHiddenRowActions: false,
                name: "",
                checkedRows: [],
                checkedRowsCount: 0,
                selectedImages: [],
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
            asd() {
                this.uploads.push({
                    FileName: null,
                    AltText: null,
                });
            },
            imageClick(id) {
                if (this.multiSelect === true) {
                    var isSelectImage = this.selectedImages.some(uploadId => uploadId === id);
                    if (isSelectImage) {
                        this.selectedImages = this.selectedImages.filter(uploadId => uploadId !== id);
                    } else {
                        this.selectedImages.push(id);
                    }
                    console.log(this.selectedImages)
                }                
            },
            checkChange() {
                if (this.checkedRows.length > 0) {
                    this.isHiddenMultiDeleteButton = true;
                    this.checkedRowsCount = this.checkedRows.length;
                }
                else {
                    this.isHiddenMultiDeleteButton = false;
                }
            },
            selectAllRows(value) {
                if (value === true) {
                    var idList = [];
                    this.terms.forEach(function (term) {
                        idList.push(term.Id);
                    });
                    this.checkedRows = idList;
                }
                else {
                    this.checkedRows = [];
                    this.isHidden = false;
                }
                this.checkChange();
            },
            singleDeleteData(id, name) {
                this.$swal({
                    title: 'Silmek istediğinize emin misiniz?',
                    text: name + " adlı terim kalıcı olarak silinecektir?",
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
                        axios.post('/admin/term/delete?term=' + id)
                            .then((response) => {
                                if (response.data.ResultStatus === 0) {
                                    this.$toast({
                                        component: ToastificationContent,
                                        props: {
                                            variant: 'success',
                                            title: 'Başarılı İşlem!',
                                            icon: 'CheckIcon',
                                            text: response.data.Message
                                        }
                                    })
                                    this.getAllData();
                                }
                                else {
                                    this.$toast({
                                        component: ToastificationContent,
                                        props: {
                                            variant: 'danger',
                                            title: 'Başarısız İşlem!',
                                            icon: 'AlertOctagonIcon',
                                            text: response.data.Message
                                        },
                                    })
                                }
                            })
                            .catch((error) => {
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
                })
            },
            multiDeleteData() {
                this.$swal({
                    title: 'Silmek istediğinize emin misiniz?',
                    text: this.checkedRowsCount + " etiket kalıcı olarak silinecektir?",
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
                        axios.post(`/admin/term/multidelete?terms=` + this.checkedRows)
                            .then((response) => {
                                console.log(response.data)
                                if (response.data.ResultStatus === 0) {
                                    this.$toast({
                                        component: ToastificationContent,
                                        props: {
                                            variant: 'success',
                                            title: 'Başarılı İşlem!',
                                            icon: 'CheckIcon',
                                            text: response.data.Message
                                        }
                                    })
                                    this.getAllData();
                                }
                                else {
                                    this.$toast({
                                        component: ToastificationContent,
                                        props: {
                                            variant: 'danger',
                                            title: 'Başarısız İşlem!',
                                            icon: 'AlertOctagonIcon',
                                            text: response.data.TermDto.Message
                                        },
                                    })
                                }
                            })
                            .catch((error) => {
                                console.log(error)
                                console.log(error.response)
                                console.log(error.request)
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
                })
                console.log(this.checkedRows);
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
    .list-group-item {
        border: 0 !important;
        margin-left: 15px;
    }

    .img-progress {
        position: relative;
        top: 25%;
    }

    .list-group-horizontal-md > .list-group-item:first-child {
        margin-left: 0 !important;
    }

    .image-list {
        max-width: 120px;
        width: 120px;
        max-height: 120px;
        height: 120px;
        padding: 5px;
        cursor: pointer;
        -webkit-box-shadow: inset 0px 0px 2px 0px rgba(0,0,0,0.75);
        -moz-box-shadow: inset 0px 0px 2px 0px rgba(0,0,0,0.75);
        box-shadow: inset 0px 0px 2px 0px rgba(0,0,0,0.75);
        border-radius: 5px !important;
    }

    .image-list:hover {
        -webkit-box-shadow: 0px 0px 2px 2px rgba(115,103,240,1);
        -moz-box-shadow: 0px 0px 2px 2px rgba(115,103,240,1);
        box-shadow: 0px 0px 2px 2px rgba(115,103,240,1);
    }

    .check-image {
        position: absolute !important;
        top: -10px;
        right: -16px;
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