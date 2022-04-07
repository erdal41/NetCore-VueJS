<template>
    <div>
        <b-modal id="modal-media"
                 scrollable
                 size="xl"
                 :ok-disabled="selectedImageId === ''"
                 :ok-title="$t('Select')"
                 ok-variant="primary"
                 :cancel-title="$t('Cancel')"
                 cancel-variant="outline-secondary"
                 @ok="selectedImage"
                 @hidden="selectedImageId = ''">
            <template v-slot:modal-header>
                <h3 class="modal-title">
                    <b>{{ $tc('Media', 0) }}</b>
                </h3>
                <div class="ml-auto">
                    <b-input-group size="sm"
                                   class="input-group-merge"
                                   v-on:mouseover="isShowSearchTextClearButton = true"
                                   v-on:mouseleave="isShowSearchTextClearButton = false">
                        <b-input-group-prepend is-text>
                            <feather-icon icon="SearchIcon" />
                        </b-input-group-prepend>
                        <b-form-input :placeholder="$t('Search') + '...'"
                                      v-model="filterText" />
                        <b-input-group-append is-text>
                            <feather-icon v-if="isShowSearchTextClearButton"
                                          icon="XIcon"
                                          v-b-tooltip.hover
                                          :title="$t('Clear')"
                                          class="cursor-pointer"
                                          @click="filterText = ''" />
                        </b-input-group-append>
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
                <div v-if="isSpinnerShow == true"
                     class="text-center mt-2 mb-2">
                    <b-spinner variant="secondary" />
                </div>
                <b-list-group v-else
                              horizontal="md">
                    <b-list-group-item v-for="upload in filteredData" :key="upload.Id"
                                       class="image-list"
                                       @click="imageClick(upload.Id, upload.FileName, upload.AltText)">
                        <b-form-checkbox v-if="multiSelect"
                                         v-model="selectedImages"
                                         name="checkbox"
                                         :value="upload.Id"
                                         class="custom-control-success check-image">
                        </b-form-checkbox>
                        <b-form-radio v-else
                                      v-model="selectedImageId"
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
                <div v-if="filteredData.length < 1 && isSpinnerShow === false"
                     class="text-center mt-2 mb-5">
                    {{ $t('No records were found', { '0': $tc('Media', 0) })  }}
                </div>
            </template>
        </b-modal>
    </div>

</template>

<script>
    import ToastificationContent from '@core/components/toastification/ToastificationContent.vue'
    import { BInputGroup, BFormInput, BInputGroupPrepend, BInputGroupAppend, BButton, BSpinner, BListGroup, BListGroupItem, BFormCheckbox, BFormRadio, BImg, VBTooltip } from 'bootstrap-vue'
    import axios from 'axios'
    import Ripple from 'vue-ripple-directive'

    export default {
        components: {
            ToastificationContent,
            BInputGroup,
            BFormInput,
            BInputGroupPrepend,
            BInputGroupAppend,
            BButton,
            BSpinner,
            BListGroup,
            BListGroupItem,
            BFormCheckbox,
            BFormRadio,
            BImg
        },
        directives: {
            'b-tooltip': VBTooltip,
            Ripple,
        },
        data() {
            return {
                isSpinnerShow: true,
                isShowSearchTextClearButton: false,
                filterText: '',
                uploads: [],
                multiSelect: false,
                selectedImages: [],
                selectedImageId: null,
                selectedImageFileName: '',
                selectedImageAltText: '',
                isHiddenMultiDeleteButton: false,
                isHiddenRowActions: false,
                checkedRows: [],
                checkedRowsCount: 0
            }
        },
        props: {
            multiSelect: {
                type: Boolean
            }
        },
        methods: {
            getAllData() {
                this.isSpinnerShow = true;
                axios.get('/admin/upload-alluploads')
                    .then((response) => {
                        if (response.data.ResultStatus === 0) {
                            this.uploads = response.data.Data.Uploads;
                        } else {
                            this.uploads = [];
                        }
                        this.isSpinnerShow = false;
                        this.filterText = "";
                        this.selectedImages = [];
                        console.log(this.multiSelect)
                    })
                    .catch((error) => {
                        this.$toast({
                            component: ToastificationContent,
                            props: {
                                variant: 'danger',
                                title: this.$t('An Error Occurred!'),
                                icon: 'AlertOctagonIcon',
                                text: this.$t('Something went wrong. Please try again.'),
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
            selectedImage() {
                this.$emit('changeImage', this.selectedImageId, this.selectedImageFileName, this.selectedImageAltText);
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