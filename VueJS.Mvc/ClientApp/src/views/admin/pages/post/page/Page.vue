<template>
    <b-row>
        <b-col md="12"
               lg="8">
            <b-card title="Makele Ekle">
                <validation-observer ref="simpleRules">
                    <b-form>
                        <b-row>
                            <b-col cols="12">
                                <b-form-group label-for="title">
                                    <validation-provider #default="{ errors }"
                                                         name="title"
                                                         vid="title"
                                                         rules="required">
                                        <b-form-input id="title"
                                                      v-model="postAddDto.Title"
                                                      :state="errors.length > 0 ? false:null"
                                                      type="text"
                                                      placeholder="Başlık" />
                                        <small class="text-danger">{{ errors[0] }}</small>
                                    </validation-provider>
                                </b-form-group>
                                <quill-editor v-model="postAddDto.Content"
                                              :options="editorOption" />
                            </b-col>
                        </b-row>
                    </b-form>
                </validation-observer>
            </b-card>

            <b-card title="Tüm Makaleler"
                    header-tag="header"
                    no-body>
                <template #header>
                    <h3 class="modal-title">
                        Galeri
                    </h3>
                    <div class="ml-auto">
                        <b-button v-b-tooltip.hover
                                  title="Galeriden resim siler."
                                  v-ripple.400="'rgba(113, 102, 240, 0.15)'"
                                  variant="flat-danger"
                                  size="small"
                                  class="mr-1">Sil</b-button>
                        <b-button v-b-tooltip.hover
                                  title="Galeri için resim seçme penceresini açar."
                                  v-ripple.400="'rgba(113, 102, 240, 0.15)'"
                                  variant="primary"
                                  size="sm">Resim Seç</b-button>
                    </div>
                </template>
            </b-card>

        </b-col>
    </b-row>
</template>

<script>
    import 'quill/dist/quill.snow.css'

    import { ValidationProvider, ValidationObserver, extend } from 'vee-validate';
    import { required, min, confirmed } from '@validations';
    import {
        BSpinner, BFormCheckbox, BButton, BCard, BCardBody, BCardTitle, BRow, BCol, BForm, BFormSelect, BFormGroup, BFormTextarea, BPagination, BInputGroup, BFormInput, BInputGroupPrepend, VBTooltip, BLink
    } from 'bootstrap-vue';
    import axios from 'axios';
    import ToastificationContent from '@core/components/toastification/ToastificationContent.vue';
    import vSelect from 'vue-select'
    import Ripple from 'vue-ripple-directive';
    import { quillEditor } from 'vue-quill-editor'

    extend('required', {
        ...required,
        message: 'Lütfen gerekli bilgileri yazınız.'
    });

    export default {
        components: {
            quillEditor,
            BSpinner,
            BCardTitle,
            BForm,
            BFormSelect,
            BButton,
            BFormCheckbox,
            BFormTextarea,
            BCard,
            BRow,
            BCol,
            BCardBody,
            BFormGroup,
            BPagination,
            BInputGroup,
            BFormInput,
            BInputGroupPrepend,
            ToastificationContent,
            ValidationProvider,
            ValidationObserver,
            vSelect,
            BLink
        },
        directives: {
            'b-tooltip': VBTooltip,
            Ripple,
        },
        data() {
            return {
                editorOption: {
                    placeholder: 'Makale İçeriği',
                    theme: 'snow'
                },
                passValue: '',
                username: '',
                required,
                min,
                confirmed,
                isSpinnerShow: true,
                perPage: 10,
                selected: '',
                selectedValue: null,
                title: "",
                postAddDto: {
                    Title: "",
                    Content: "",
                    ParentId: null,
                    Description: '',
                    TermType: 'tag'
                },
                seoObjectSettingAddDto: {
                    SeoTitle: this.Name
                },
                hoveredRow: null
            }
        },
        methods: {
            validationForm() {
                this.$refs.simpleRules.validate().then(success => {
                    if (success) {
                        axios.post('/admin/term/new',
                            {
                                PostAddDto: this.postAddDto,
                                SeoObjectSettingAddDto: this.seoObjectSettingAddDto
                            })
                            .then((response) => {
                                console.log(response.data);
                                if (response.data.PostDto.ResultStatus === 0) {
                                    this.$toast({
                                        component: ToastificationContent,
                                        props: {
                                            variant: 'success',
                                            title: 'Başarılı İşlem!',
                                            icon: 'CheckIcon',
                                            text: response.data.PostDto.Message
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
                                            text: response.data.PostDto.Message
                                        },
                                    })
                                }
                            })
                            .catch((error) => {
                                console.log(error);
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
        },
        computed: {
        },
        mounted() {
        }
    }
</script>

<style lang="scss">
    @import '@core/scss/vue/libs/vue-select.scss';

    .ql-editor {
        min-height: 500px !important;
    }
</style>