<template>
    <div>
        <b-modal id="upload-modal"
                 ref="upload-modal"
                 title="Medya Düzenle"
                 :no-close-on-backdrop="true"
                 @show="getData"
                 @hidden="mediaPageGetAllData">
            <template #modal-header>
                <div class="content-header-left"
                     cols="12"
                     md="6">
                    <h3 class="modal-title float-left">
                        {{ $t('Media Details') }}
                    </h3>
                </div>
                <div class="content-header-right text-md-right d-md-block d-none"
                     md="6"
                     cols="12">
                    <b-button-group size="sm">
                        <b-button variant="flat-secondary"
                                  class="btn-icon rounded-circle"
                                  :disabled="isDisabledPreviousButton"
                                  @click="prevNextButtonClick('Pre')">
                            <feather-icon icon="ChevronLeftIcon" />
                        </b-button>
                        <b-button variant="flat-secondary"
                                  class="btn-icon rounded-circle"
                                  :disabled="isDisabledNextButton"
                                  @click="prevNextButtonClick('Next')">
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
            <template #modal-footer>
                <b-button variant="flat-danger"
                          size="sm"
                          @click="deleteData">
                    {{ $t('Delete Permanently') }}
                </b-button>
                <b-button variant="primary"
                          size="sm"
                          @click="updateData">
                    {{ $t('Update') }}
                </b-button>
            </template>
            <div class="media-details container-fluid h-100 p-0">

                <div class="media-view">
                    <b-overlay :show="showOverlay"
                               size="sm">
                        <div class="image-view">
                            <b-img :src="uploadUpdateDto.FileName == null ? '' : require('@/assets/images/media/' + uploadUpdateDto.FileName)" fluid></b-img>
                        </div>
                    </b-overlay>
                </div>
                <div class="media-info">
                    <b-overlay :show="showOverlay"
                               size="sm">
                        <div class="details">
                            <span><strong>{{ $t('Uploaded Date') }}: </strong>{{createdDate}}</span><br />
                            <span><strong>{{ $t('Uploaded By') }}: </strong>{{ uploadUpdateDto.User.UserName}}</span><br />
                            <span><strong>{{ $t('File Name') }}: </strong>{{ uploadUpdateDto.FileName}}</span><br />
                            <span><strong>{{ $t('File Type') }}: </strong>{{ uploadUpdateDto.ContentType}}</span><br />
                            <span><strong>{{ $t('File Size') }}: </strong>{{sizeConvert}}</span><br />
                            <span><strong>{{ $t('Dimensions') }}: </strong>{{ uploadUpdateDto.Width }}x{{ uploadUpdateDto.Height }}px</span>
                        </div>
                        <div class="settings">
                            <b-form-group label-cols="4"
                                          label-size="sm"
                                          :label="$t('Alternative Text')"
                                          label-for="fileUrl">
                                <b-form-input id="fileUrl"
                                              v-model="uploadUpdateDto.AltText"
                                              type="text"
                                              size="sm"></b-form-input>
                            </b-form-group>
                            <b-form-group label-cols="4"
                                          label-size="sm"
                                          :label="$t('Title')"
                                          label-for="title">
                                <b-form-input id="title"
                                              v-model="uploadUpdateDto.Title"
                                              type="text"
                                              size="sm"></b-form-input>
                            </b-form-group>
                            <b-form-group label-cols="4"
                                          label-size="sm"
                                          :label="$t('Sub Title')"
                                          label-for="subTitle">
                                <b-form-input id="subTitle"
                                              v-model="uploadUpdateDto.SubTitle"
                                              type="text"
                                              size="sm"></b-form-input>
                            </b-form-group>
                            <b-form-group label-cols="4"
                                          label-size="sm"
                                          :label="$t('Description')"
                                          label-for="description">
                                <b-form-textarea id="description"
                                                 v-model="uploadUpdateDto.Description"
                                                 size="sm"></b-form-textarea>
                            </b-form-group>
                            <b-form-group label-cols="4"
                                          label-size="sm"
                                          :label="$t('File URL')"
                                          label-for="fileUrl">
                                <b-form-input id="fileUrl"
                                              v-model="fileUrl"
                                              type="text"
                                              size="sm"
                                              readonly></b-form-input>
                            </b-form-group>
                        </div>
                    </b-overlay>
                </div>
            </div>
        </b-modal>
    </div>
</template>

<script>
    import ToastificationContent from '@core/components/toastification/ToastificationContent.vue'
    import moment from 'moment'
    import { BRow, BCol, BOverlay, BImg, BFormGroup, BFormInput, BFormTextarea, BButtonGroup, BButton } from 'bootstrap-vue'
    import axios from 'axios'

    export default {
        components: {
            ToastificationContent,
            BRow,
            BCol,
            BOverlay,
            BImg,
            BFormGroup,
            BFormInput,
            BFormTextarea,
            BButtonGroup,
            BButton
        },
        props: {
            uploadId: {
                type: Number,
                required: true,
            },
            filteredData: {
                type: Array,
                default: () => [],
            },
        },
        data() {
            return {
                showOverlay: false,
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
                fileUrl: '',
                uploadIds: [],
                uploadIndex: null,
                isDisabledPreviousButton: false,
                isDisabledNextButton: false,
                isDeleted: false,
            }
        },
        methods: {
            getAllData() {
                this.uploadIds = [];
                this.filteredData.forEach(upload => {
                    this.uploadIds.push(upload.Id);
                });
            },
            getData() {
                this.showOverlay = true;
                this.getAllData();
                this.isDeleted = false;
                axios.get('/admin/upload-edit', {
                    params: {
                        uploadId: this.uploadId
                    }
                }).then((response) => {
                    console.log(response.data);
                    if (response.data.UploadUpdateDto.ResultStatus === 0) {
                        this.uploadUpdateDto.Id = response.data.UploadUpdateDto.Data.Id;
                        this.uploadUpdateDto.AltText = response.data.UploadUpdateDto.Data.AltText;
                        this.uploadUpdateDto.ContentType = response.data.UploadUpdateDto.Data.ContentType;
                        this.uploadUpdateDto.CreatedDate = response.data.UploadUpdateDto.Data.CreatedDate;
                        this.uploadUpdateDto.Description = response.data.UploadUpdateDto.Data.Description;
                        this.uploadUpdateDto.FileName = response.data.UploadUpdateDto.Data.FileName;
                        this.uploadUpdateDto.FileUrl = response.data.UploadUpdateDto.Data.FileUrl;
                        this.uploadUpdateDto.Height = response.data.UploadUpdateDto.Data.Height;
                        this.uploadUpdateDto.SubTitle = response.data.UploadUpdateDto.Data.SubTitle;
                        this.uploadUpdateDto.Title = response.data.UploadUpdateDto.Data.Title;
                        this.uploadUpdateDto.User = response.data.UploadUpdateDto.Data.User;
                        this.uploadUpdateDto.UserId = response.data.UploadUpdateDto.Data.UserId;
                        this.uploadUpdateDto.Width = response.data.UploadUpdateDto.Data.Width;
                        this.uploadUpdateDto.Size = response.data.UploadUpdateDto.Data.Size;
                        this.createdDate = moment(new Date(response.data.UploadUpdateDto.Data.CreatedDate)).format('DD.MM.YYYY');

                        var imgPath = require('@/assets/images/media/' + response.data.UploadUpdateDto.Data.FileName);
                        if (imgPath.includes('png')) {
                            this.fileUrl = imgPath;
                        } else {
                            this.fileUrl = location.origin + imgPath;
                        }


                        var size = response.data.UploadUpdateDto.Data.Size;
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

                        this.uploadIndex = this.uploadIds.indexOf(this.uploadUpdateDto.Id);
                        this.isDisabledPreviousButton = this.uploadIndex == 0 ? true : false;
                        this.isDisabledNextButton = this.uploadIndex == this.uploadIds.length - 1 ? true : false;
                        this.showOverlay = false;
                    }
                })
            },
            getPrevNextData(id) {
                this.showOverlay = true;
                axios.get('/admin/upload-edit', {
                    params: {
                        uploadId: id
                    }
                }).then((response) => {
                    if (response.data.UploadUpdateDto.ResultStatus === 0) {
                        this.uploadUpdateDto.Id = response.data.UploadUpdateDto.Data.Id;
                        this.uploadUpdateDto.AltText = response.data.UploadUpdateDto.Data.AltText;
                        this.uploadUpdateDto.ContentType = response.data.UploadUpdateDto.Data.ContentType;
                        this.uploadUpdateDto.CreatedDate = response.data.UploadUpdateDto.Data.CreatedDate;
                        this.uploadUpdateDto.Description = response.data.UploadUpdateDto.Data.Description;
                        this.uploadUpdateDto.FileName = response.data.UploadUpdateDto.Data.FileName;
                        this.uploadUpdateDto.FileUrl = response.data.UploadUpdateDto.Data.FileUrl;
                        this.uploadUpdateDto.Height = response.data.UploadUpdateDto.Data.Height;
                        this.uploadUpdateDto.SubTitle = response.data.UploadUpdateDto.Data.SubTitle;
                        this.uploadUpdateDto.Title = response.data.UploadUpdateDto.Data.Title;
                        this.uploadUpdateDto.User = response.data.UploadUpdateDto.Data.User;
                        this.uploadUpdateDto.UserId = response.data.UploadUpdateDto.Data.UserId;
                        this.uploadUpdateDto.Width = response.data.UploadUpdateDto.Data.Width;
                        this.uploadUpdateDto.Size = response.data.UploadUpdateDto.Data.Size;
                        this.createdDate = moment(new Date(response.data.UploadUpdateDto.Data.CreatedDate)).format('DD.MM.YYYY');

                        var imgPath = require('@/assets/images/media/' + response.data.UploadUpdateDto.Data.FileName)
                        if (imgPath.includes('png')) {
                            this.fileUrl = imgPath;
                        } else {
                            this.fileUrl = location.origin + imgPath;
                        }

                        var size = response.data.UploadUpdateDto.Data.Size;
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
                        this.showOverlay = false;
                    }
                })
            },
            prevNextButtonClick: function (type) {
                this.uploadIndex = this.uploadIds.indexOf(this.uploadUpdateDto.Id);

                if (type === 'Pre') {
                    if (this.uploadIndex != 0) {
                        this.uploadIndex--;
                        this.isDisabledNextButton = false;
                        this.uploadUpdateDto.Id = this.uploadIds[this.uploadIndex]
                        this.getPrevNextData(this.uploadUpdateDto.Id);
                    }
                }
                else if (type === 'Next') {
                    if (this.uploadIndex !== this.uploadIds.length - 1) {
                        this.uploadIndex++;
                        this.isDisabledPreviousButton = false;
                        this.uploadUpdateDto.Id = this.uploadIds[this.uploadIndex];
                        this.getPrevNextData(this.uploadUpdateDto.Id);
                    }

                }
                this.isDisabledPreviousButton = this.uploadIndex == 0 ? true : false;
                this.isDisabledNextButton = this.uploadIndex == this.uploadIds.length - 1 ? true : false;
            },
            handleKeydown(e) {
                switch (e.keyCode) {
                    case 37:
                        this.prevNextButtonClick('Pre');
                        break;
                    case 39:
                        this.prevNextButtonClick('Next');
                        break;
                }
            },
            updateData() {
                console.log(this.uploadUpdateDto);
                axios.post('/admin/upload-edit',
                    {
                        UploadUpdateDto: this.uploadUpdateDto
                    })
                    .then((response) => {
                        console.log(response.data);
                        if (response.data.UploadDto.ResultStatus === 0) {
                            this.$toast({
                                component: ToastificationContent,
                                props: {
                                    variant: 'success',
                                    title: this.$t('Successful Transaction!'),
                                    icon: 'CheckIcon',
                                    text: this.$t('Updated', { '0': this.uploadUpdateDto.FileName, '1': this.$tc('Media', 0) })
                                }
                            });
                        }
                        else {
                            this.$toast({
                                component: ToastificationContent,
                                props: {
                                    variant: 'danger',
                                    title: this.$t('Failed Transaction!'),
                                    icon: 'AlertOctagonIcon',
                                    text: this.$t('No such data was found.', { '0': this.$tc('Media', 0) })
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
                                title: this.$t('An Error Occurred!'),
                                icon: 'AlertOctagonIcon',
                                text: this.$t('Something went wrong. Please try again.'),
                            },
                        })
                    });
            },
            deleteData() {
                this.$swal({
                    title: this.$t('Are you sure you want to delete?'),
                    text: this.$t('It will be permanently deleted. Do you want to continue?', { '0': this.uploadUpdateDto.FileName, '1': this.$tc('Media', 0) }),
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonText: this.$t('Yes'),
                    cancelButtonText: this.$t('No'),
                    customClass: {
                        confirmButton: 'btn btn-primary',
                        cancelButton: 'btn btn-outline-danger ml-1',
                    },
                    buttonsStyling: false,
                }).then(result => {
                    if (result.value) {
                        console.log(this.uploadUpdateDto.Id);
                        axios.post('/admin/upload-delete?uploadId=' + this.uploadUpdateDto.Id)
                            .then((response) => {
                                this.getAllData();
                                this.isDeleted = true;
                                console.log(response.data)
                                if (response.data.ResultStatus === 0) {
                                    this.$toast({
                                        component: ToastificationContent,
                                        props: {
                                            variant: 'success',
                                            title: this.$t('Successful Transaction!'),
                                            icon: 'CheckIcon',
                                            text: response.data.Data.FileFullName + ' adlı dosya silindi.'
                                        }
                                    });
                                    this.$refs['upload-modal'].hide()
                                }
                                else {
                                    this.$toast({
                                        component: ToastificationContent,
                                        props: {
                                            variant: 'danger',
                                            title: this.$t('Failed Transaction!'),
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
                                        title: this.$t('An Error Occurred!'),
                                        icon: 'AlertOctagonIcon',
                                        text: this.$t('Something went wrong. Please try again.'),
                                    },
                                })
                            });
                    }
                })
            },
            mediaPageGetAllData() {
                this.$emit('mediaGetAllData', this.uploadUpdateDto.Id, this.isDeleted);
            }
        },
        beforeMount() {
            window.addEventListener('keydown', this.handleKeydown, null);
        },
        beforeDestroy() {
            window.removeEventListener('keydown', this.handleKeydown);
        }
    }
</script>

<style>

    #upload-modal .modal-dialog {
        max-width: 100%;
        margin: 0;
        position: fixed;
        top: 30px;
        left: 30px;
        right: 30px;
        bottom: 30px;
        z-index: 160000;
    }

        #upload-modal .modal-dialog .modal-content {
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            overflow: auto;
            min-height: 300px;
            box-shadow: 0 5px 15px rgb(0 0 0 / 70%);
            background: #fff;
        }

            #upload-modal .modal-dialog .modal-content .modal-header {
                padding: 0 0 0 10px;
            }

            #upload-modal .modal-dialog .modal-content .modal-body {
                border-bottom: none;
                left: 0;
                top: 30px;
                position: absolute;
                left: 0;
                right: 0;
                bottom: 61px;
                height: auto;
                width: auto;
                margin: 0;
                overflow: auto;
                background: #fff;
                border-top: 1px solid #dcdcde;
            }

                #upload-modal .modal-dialog .modal-content .modal-body .media-details {
                    position: absolute;
                    overflow: auto;
                    top: 0;
                    bottom: 0;
                    right: 0;
                    left: 0;
                    box-shadow: inset 0 4px 4px -4px rgb(0 0 0 / 10%);
                }

                    #upload-modal .modal-dialog .modal-content .modal-body .media-details .media-view {
                        float: left;
                        width: 65%;
                        height: 100%;
                    }

                        #upload-modal .modal-dialog .modal-content .modal-body .media-details .media-view .image-view {
                            box-sizing: border-box;
                            padding: 16px;
                            height: 100%;
                        }

                            #upload-modal .modal-dialog .modal-content .modal-body .media-details .media-view .image-view img {
                                display: block;
                                margin: 0 auto 16px;
                                max-width: 100%;
                                max-height: 90%;
                                max-height: calc(100% - 42px);
                                background-image: linear-gradient(45deg,#c3c4c7 25%,transparent 25%,transparent 75%,#c3c4c7 75%,#c3c4c7),linear-gradient(45deg,#c3c4c7 25%,transparent 25%,transparent 75%,#c3c4c7 75%,#c3c4c7);
                                background-position: 0 0,10px 10px;
                                background-size: 20px 20px;
                            }

                    #upload-modal .modal-dialog .modal-content .modal-body .media-details .media-info {
                        overflow: auto;
                        box-sizing: border-box;
                        margin-bottom: 0;
                        padding: 12px 16px 0;
                        width: 35%;
                        height: 100%;
                        box-shadow: inset 0 4px 4px -4px rgb(0 0 0 / 10%);
                        border-bottom: 0;
                        border-left: 1px solid #dcdcde;
                        background: #f6f7f7;
                    }

                        #upload-modal .modal-dialog .modal-content .modal-body .media-details .media-info .details {
                            position: relative;
                            overflow: hidden;
                            float: none;
                            margin-bottom: 15px;
                            padding-bottom: 15px;
                            border-bottom: 1px solid #dcdcde;
                            font-size: 11px;
                            max-width: 100%;
                        }

                        #upload-modal .modal-dialog .modal-content .modal-body .media-details .media-info .settings {
                        }

                        #upload-modal .modal-dialog .modal-content .modal-body .media-details .media-info .actions {
                            margin-bottom: 16px;
                        }


            #upload-modal .modal-dialog .modal-content .modal-footer {
                position: absolute;
                bottom: 0;
                right: 0;
            }
</style>