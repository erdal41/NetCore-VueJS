<template>
    <div>
        <b-modal id="modal-media"
                 scrollable
                 size="xl"
                 ok-title="Seç"
                 ok-variant="primary"
                 cancel-title="İptal"
                 cancel-variant="outline-secondary"
                 @ok="selectedImage( )">
            <template v-slot:modal-header>
                <h3 class="modal-title">
                    <b>Medya</b>
                </h3>
                <div class="ml-auto">
                    <b-input-group size="sm">
                        <b-input-group-prepend is-text>
                            <feather-icon icon="SearchIcon" />
                        </b-input-group-prepend>
                        <b-form-input placeholder="Ara..."
                                      v-model="filterText" />
                    </b-input-group>
                </div>
                <div class="ml-auto">
                    <b-button v-ripple.400="'rgba(113, 102, 240, 0.15)'"
                              variant="fade-secondary"
                              class="btn-icon mr-1"
                              size="sm">
                        <feather-icon icon="RotateCcwIcon" />
                    </b-button>
                    <b-button v-ripple.400="'rgba(113, 102, 240, 0.15)'"
                              variant="fade-secondary"
                              class="btn-icon"
                              size="sm"
                              @click="$bvModal.hide('modal-media')">
                        <feather-icon icon="XIcon" />
                    </b-button>
                </div>
            </template>
            <template>
                <b-list-group horizontal="md">
                    <b-list-group-item v-for="upload in filteredData" :key="upload.Id"
                                       class="image-list"
                                       @click="imageClick(upload.Id, upload.FileName, upload.AltText)">
                        <b-form-radio v-model="selectedImageId"
                                      name="radio"
                                      :value="upload.Id"
                                      class="custom-control-success check-image">
                        </b-form-radio>
                        <b-img rounded
                               :key="upload.Id"
                               :src="upload.FileName == null ? '' : require('@/assets/images/media/' + upload.FileName)"
                               :alt="upload.AltText"
                               class="d-inline-block select-image" />
                    </b-list-group-item>
                </b-list-group>
            </template>
        </b-modal>
    </div>

</template>

<script>
    import {
        BFormCheckbox, BFormRadio, BButton, BImg, BRow, BCol, BFormGroup, BListGroup, BListGroupItem, BPagination, BInputGroup, BFormInput, BInputGroupPrepend, BInputGroupAppend, VBTooltip, BLink
    } from 'bootstrap-vue'
    //import { codeRowDetailsSupport } from './code'
    import axios from 'axios'
    import ToastificationContent from '@core/components/toastification/ToastificationContent.vue'
    import Ripple from 'vue-ripple-directive'

    export default {
        components: {
            BInputGroupPrepend,
            BImg,
            BButton,
            BFormCheckbox,
            BFormRadio,
            BRow,
            BCol,
            BFormGroup,
            BPagination,
            BInputGroup,
            BFormInput,
            BInputGroupAppend,
            ToastificationContent,
            BLink,
            BListGroup,
            BListGroupItem
        },
        directives: {
            'b-tooltip': VBTooltip,
            Ripple,
        },
        data() {
            return {
                perPage: 10,
                pageOptions: [10, 20, 50, 100],
                totalRows: 1,
                currentPage: 1,
                filter: null,
                filterText: '',
                filterOn: [],
                uploads: [],
                multiSelect: false,
                selectedImageId: null,
                selectedImageFileName: '',
                selectedImageAltText: '',
                isHiddenMultiDeleteButton: false,
                isHiddenRowActions: false,
                checkedRows: [],
                checkedRowsCount: 0
            }
        },
        methods: {
            selectedImage() {
                this.$emit('changeImage', this.selectedImageId, this.selectedImageFileName, this.selectedImageAltText);
                this.selectedImageId = '';
            },
            imageClick(id, name, altText) {
                this.selectedImageId = id;
                this.selectedImageFileName = name;
                this.selectedImageAltText = altText;
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
                    this.uploads.forEach(function (upload) {
                        idList.push(upload.Id);
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
                    text: name + " isimli etiket kalıcı olarak silinecektir?",
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
                        axios.post('/admin/upload/delete?uploadId=' + id)
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
                                            text: response.data.TermDto.Message
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
                        axios.post(`/admin/upload/delete`, {
                            termsId: this.checkedRows
                        })
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
                                            text: response.data.TermDto.Message
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
            getAllData() {
                axios.get('/admin/upload-alluploads')
                    .then((response) => {
                        if (response.data.ResultStatus === 0) {
                            this.uploads = response.data.Data.Uploads;
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

    .image-list {
        max-width: 130px;
        width: 130px;
        max-height: 130px;
        height: 130px;
        padding: 5px;
        cursor: pointer;
    }

    .image-list .check-image {
        position: absolute;
        right: -6px;
        top: 1px;
    }

    .image-list .select-image {
        max-width: 100%;
        max-height: 100%;
        position: relative;
        top: 50%;
        left: 50%;
        transform: translateX(-50%) translateY(-50%);
    }
</style>