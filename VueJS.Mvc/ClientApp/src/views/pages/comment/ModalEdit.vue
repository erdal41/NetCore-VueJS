<template>
    <div>
        <b-modal id="modal-edit"
                 cancel-variant="outline-secondary"
                 ok-title="Güncelle"
                 cancel-title="İptal"
                 centered
                 title="Yorum Düzenle"
                 :no-close-on-backdrop="true"
                 @show="getData"
                 @hidden="commentPageGetAllData"
                 @ok="validationForm">
            <template #modal-header>
                <div class="content-header-left"
                     cols="6">
                    <h3 class="modal-title float-left">
                        Yorum Önizleme
                    </h3>
                </div>
                <div class="content-header-right text-md-right d-md-block d-none"
                     cols="6">
                    <b-form-group>
                        <b-form-checkbox v-model="commentUpdateDto.CommentStatus"
                                         class="custom-control-success"
                                         value="1"
                                         unchecked-value="0"
                                         switch
                                         inline>
                            <b-badge v-if="commentUpdateDto.CommentStatus == 1"
                                     variant="success">
                                Onaylandı
                            </b-badge>
                            <b-badge v-if="commentUpdateDto.CommentStatus == 0"
                                     variant="warning">
                                İnceleme Bekliyor
                            </b-badge>
                        </b-form-checkbox>
                    </b-form-group>
                </div>
            </template>
            <validation-observer ref="simpleRules">
                <b-form>
                    <b-row>
                        <b-col cols="6">
                            <b-form-group label="Yorum Yapan">
                                <b-form-input disabled
                                              :value="commentUpdateDto.Name"></b-form-input>
                            </b-form-group>
                        </b-col>
                        <b-col cols="6">
                            <b-form-group label="Email">
                                <b-form-input disabled
                                              :value="commentUpdateDto.Email"></b-form-input>
                            </b-form-group>
                        </b-col>
                        <b-col cols="12">
                            <b-form-group label-for="comment-text">
                                <validation-provider #default="{ errors }"
                                                     name="comment-text"
                                                     rules="required">
                                    <b-form-textarea id="comment-text"
                                                     v-model="commentUpdateDto.Text"
                                                     placeholder="Yorum İçeriği"
                                                     rows="3" />
                                    <small class="text-danger">{{ errors[0] }}</small>
                                </validation-provider>
                            </b-form-group>
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
    import ToastificationContent from '@core/components/toastification/ToastificationContent.vue'
    import { BRow, BCol, BBadge, BForm, BFormGroup, BFormCheckbox, BFormInput, BFormTextarea } from 'bootstrap-vue'
    import axios from 'axios'

    extend('required', {
        ...required,
        message: 'Lütfen gerekli bilgileri yazınız.'
    });

    export default {
        components: {
            ValidationProvider,
            ValidationObserver,
            ToastificationContent,
            BRow,
            BCol,
            BBadge,
            BForm,
            BFormGroup,
            BFormCheckbox,
            BFormInput,
            BFormTextarea,
        },
        props: {
            commentId: {
                type: Number,
                required: true,
            },
        },
        data() {
            return {
                required,
                asd: '',
                commentUpdateDto: {
                    Id: '',
                    Name: '',
                    Text: '',
                    Email: '',
                    IsPersonalInformationsSharing: false,
                    ParentId: null,
                    ModifiedDate: '',
                    UserId: null,
                    CommentStatus: '',
                    PostId: null
                },
            }
        },
        methods: {
            commentPageGetAllData() {
                this.$emit('commentGetAllData');
            },
            getData() {
                axios.get('/admin/comment-edit', {
                    params: {
                        commentId: this.commentId
                    }
                }).then((response) => {
                    console.log(response.data);
                    if (response.data.CommentUpdateDto.ResultStatus === 0) {
                        this.commentUpdateDto.Id = response.data.CommentUpdateDto.Data.Id;
                        this.commentUpdateDto.Name = response.data.CommentUpdateDto.Data.Name;
                        this.commentUpdateDto.Text = response.data.CommentUpdateDto.Data.Text;
                        this.commentUpdateDto.Email = response.data.CommentUpdateDto.Data.Email;
                        this.commentUpdateDto.IsPersonalInformationsSharing = response.data.CommentUpdateDto.Data.IsPersonalInformationsSharing;
                        this.commentUpdateDto.ParentId = response.data.CommentUpdateDto.Data.ParentId;
                        this.commentUpdateDto.ModifiedDate = response.data.CommentUpdateDto.Data.ModifiedDate;
                        this.commentUpdateDto.UserId = response.data.CommentUpdateDto.Data.UserId;
                        this.commentUpdateDto.CommentStatus = response.data.CommentUpdateDto.Data.CommentStatus;
                        this.commentUpdateDto.PostId = response.data.CommentUpdateDto.Data.PostId;
                    }                    
                })
            },
            validationForm() {
                this.$refs.simpleRules.validate().then(success => {
                    if (success) {
                        axios.post('/admin/comment-edit',
                            {
                                CommentUpdateDto: this.commentUpdateDto
                            })
                            .then((response) => {
                                console.log(response.data);
                                if (response.data.CommentDto.ResultStatus === 0) {
                                    this.$toast({
                                        component: ToastificationContent,
                                        props: {
                                            variant: 'success',
                                            title: 'Başarılı İşlem!',
                                            icon: 'CheckIcon',
                                            text: response.data.CommentDto.Message
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
                                            text: response.data.CommentDto.Message
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
                })
            },
        }
    }
</script>