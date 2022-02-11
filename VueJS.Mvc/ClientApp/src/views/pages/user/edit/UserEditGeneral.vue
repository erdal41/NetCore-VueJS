<template>
    <b-card>
        <modal-media v-bind:show="modalShow"
                     @changeImage="imageChange"
                     ref="modalMedia"></modal-media>
        <b-row>
            <b-col cols="12">
                <div class="d-flex align-items-center mb-2">
                    <feather-icon icon="UserIcon"
                                  size="18" />
                    <h4 class="mb-0 ml-75">
                        Genel Bilgiler
                    </h4>
                </div>
            </b-col>
        </b-row>
        <!-- form -->
        <b-form class="mt-2">
            <b-row>

                <b-col lg="12"
                       md="12"
                       class="d-flex justify-content-center mb-3">
                    <div class="image-thumb">
                        <b-img id="profileImage"
                               v-bind:src="profileImage.fileName == null ? noImage : require('@/assets/images/media/' + profileImage.fileName)"
                               :alt="profileImage.fileName == null ? '' : profileImage.fileName.altText"
                               rounded />
                        <b-button id="selectImage"
                                  v-ripple.400="'rgba(186, 191, 199, 0.15)'"
                                  variant="relief-primary"
                                  size="sm"
                                  class="btn-icon rounded-circle"
                                  v-b-modal.modal-media>
                            <feather-icon icon="Edit2Icon"
                                          size="11" />

                        </b-button>


                        <!--/ upload button -->
                        <!-- reset -->
                        <b-button id="removeImage"
                                  v-ripple.400="'rgba(186, 191, 199, 0.15)'"
                                  variant="relief-secondary"
                                  size="sm"
                                  class="btn-icon rounded-circle"
                                  @click="removeImage">
                            <feather-icon icon="XIcon"
                                          size="11" />
                        </b-button>
                        <b-form-input type="text"
                                      hidden
                                      v-model="profileImage.fileName == null ? '' : profileImage.fileName.id"></b-form-input>
                    </div>
                </b-col>

                <b-col sm="6">
                    <b-form-group label="Ad"
                                  label-for="Ad">
                        <b-form-input v-model="userUpdateDto.FirstName"
                                      name="firstname"
                                      placeholder="Ad" />
                    </b-form-group>
                </b-col>
                <b-col sm="6">
                    <b-form-group label="Soyad"
                                  label-for="Soyad">
                        <b-form-input v-model="userUpdateDto.LastName"
                                      name="lastname"
                                      placeholder="Soyad" />
                    </b-form-group>
                </b-col>
                <b-col sm="6">
                    <b-form-group label="Yalnızca harfler, sayılar, kısa çizgiler veya alt çizgilerden oluşabilir."
                                  label-for="username">
                        <validation-provider #default="{ errors }"
                                             name="username"
                                             rules="required|minmax|alpha-dash">
                            <b-form-input id="username"
                                          v-model="userUpdateDto.UserName"
                                          :state="errors.length > 0 ? false:null"
                                          placeholder="Kullanıcı Adı"
                                          name="username" />
                            <small class="text-danger">{{ errors[0] }}</small>
                        </validation-provider>
                    </b-form-group>
                </b-col>
                <b-col sm="6">
                    <b-form-group label="E-posta Adresi"
                                  label-for="email">
                        <validation-provider #default="{ errors }"
                                             name="email"
                                             rules="required|email">
                            <b-form-input id="email"
                                          v-model="userUpdateDto.Email"
                                          name="email"
                                          :state="errors.length > 0 ? false:null"
                                          placeholder="E-posta Adresi"
                                          autofocus />
                            <small class="text-danger">{{ errors[0] }}</small>
                        </validation-provider>
                    </b-form-group>
                </b-col>

                <b-col cols="12">
                    <hr class="my-2">
                </b-col>

                <b-col cols="12">
                    <b-button v-ripple.400="'rgba(255, 255, 255, 0.15)'"
                              variant="primary"
                              class="mt-1 mr-1"
                              @click.prevent="updateData">
                        Güncelle
                    </b-button>
                    <b-button v-ripple.400="'rgba(186, 191, 199, 0.15)'"
                              type="reset"
                              class="mt-1 ml-25"
                              variant="outline-secondary"
                              @click.prevent="resetForm">
                        Tüm Değişiklikleri İptal Et
                    </b-button>
                </b-col>
            </b-row>
        </b-form>
    </b-card>
</template>

<script>
    import { ValidationProvider, ValidationObserver, extend } from 'vee-validate'
    import { required, alpha, email } from '@validations'

    import {
        BButton, BForm, BFormGroup, BFormInput, BInputGroup, BInputGroupPrepend, BRow, BCol, BCard, BCardText, BImg,
    } from 'bootstrap-vue'
    import Ripple from 'vue-ripple-directive'
    import ModalMedia from '@/views/pages/media/ModalMedia.vue';
    import { useInputImageRenderer } from '@core/comp-functions/forms/form-utils'
    import { ref } from '@vue/composition-api'
    import axios from 'axios';
    import ToastificationContent from '@core/components/toastification/ToastificationContent.vue';

    extend('required', {
        ...required,
        message: 'Lütfen gerekli bilgileri yazınız.'
    });

    extend('minmax', {
        validate(value) {
            return value.length >= 6 && value.length <= 30;
        },
        message: 'Kullanıcı Adı, en az 6 ve en fazla 30 karakter olmalıdır.'
    });

    extend('alpha-dash', {
        ...alpha,
        message: 'Kullanıcı Adı, harfler, sayılar, kısa çizgiler veya alt çizgilerden oluşabilir.'
    });

    extend('email', {
        ...email,
        message: 'Lütfen geçerli bir e-posta adresi giriniz.'
    });

    export default {
        components: {
            ModalMedia,
            BButton,
            BForm,
            BImg,
            BFormGroup,
            BFormInput,
            BInputGroup,
            BInputGroupPrepend,
            BRow,
            BCol,
            BCard,
            BCardText,
            ValidationProvider,
            ValidationObserver,
        },
        directives: {
            Ripple,
        },
        props: {
            generalData: {
                type: Object,
                default: () => { },
            },
        },
        data() {
            return {
                localOptions: JSON.parse(JSON.stringify(this.generalData)),
                modalShow: Boolean,
                noImage: require('@/assets/images/default/default-post-image.jpg'),
                profileImage: {
                    id: null,
                    fileName: null,
                    altText: null,
                },
                required,
                alpha,
                email,
                userUpdateDto: {
                    Id: this.$route.query.edit,
                    ProfileImageId: null,
                    FirstName: null,
                    LastName: null,
                    UserName: null,
                    Email: null,
                    About: null,
                    WebSiteLink: null,
                    PhoneNumber: null,
                    FacebookLink: null,
                    TwitterLink: null,
                    InstagramLink: null,
                    LinkedInLink: null,
                    YoutubeLink: null,
                    GitHubLink: null,
                }
            }
        },
        methods: {
            updateData() {
                this.userUpdateDto.ProfileImageId = this.profileImage.id;
                axios.post('/admin/user-edit',
                    {
                        UserUpdateDto: this.userUpdateDto
                    }).then((response) => {
                        if (response.data.User != null) {
                            this.$toast({
                                component: ToastificationContent,
                                props: {
                                    variant: 'success',
                                    title: 'Başarılı İşlem!',
                                    icon: 'CheckIcon',
                                    text: response.data.User.UserName + ' adlı kullanıcı güncellendi.'
                                }
                            })
                        }
                        else {
                            this.$toast({
                                component: ToastificationContent,
                                props: {
                                    variant: 'danger',
                                    title: 'Başarısız İşlem!',
                                    icon: 'AlertOctagonIcon',
                                    text: 'Kullanıcı güncellenirken hata oluştu.',
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
                                text: 'Hata oluştu. Lütfen tekrar deneyin.',
                            },
                        })
                    });
            },
            getAllData() {
                this.userUpdateDto.FirstName = this.localOptions.FirstName;
                this.userUpdateDto.LastName = this.localOptions.LastName;
                this.userUpdateDto.UserName = this.localOptions.UserName;
                this.userUpdateDto.Email = this.localOptions.Email;
                this.userUpdateDto.About = this.localOptions.About;
                this.userUpdateDto.WebsiteLink = this.localOptions.WebsiteLink;
                this.userUpdateDto.PhoneNumber = this.localOptions.PhoneNumber;
                this.userUpdateDto.FacebookLink = this.localOptions.FacebookLink;
                this.userUpdateDto.TwitterLink = this.localOptions.TwitterLink;
                this.userUpdateDto.InstagramLink = this.localOptions.InstagramLink;
                this.userUpdateDto.LinkedInLink = this.localOptions.LinkedInLink;
                this.userUpdateDto.YoutubeLink = this.localOptions.YoutubeLink;
                this.userUpdateDto.GitHubLink = this.localOptions.GitHubLink;

                console.log(this.localOptions.ProfileImage)

                if (this.localOptions.ProfileImage != null) {
                    this.profileImage.id = this.localOptions.ProfileImage.Id;
                    this.profileImage.fileName = this.localOptions.ProfileImage.FileName;
                    this.profileImage.altText = this.localOptions.ProfileImage.AltText;
                }
                else {
                    this.removeImage();
                }
            },
            resetForm() {
                this.getAllData();
            },
            imageChange(id, name, altText) {
                console.log(id);
                this.profileImage.id = id;
                this.profileImage.fileName = name;
                this.profileImage.altText = altText;
            },
            selectImage: function (e) {
                this.modalShow = true;
            },
            removeImage() {
                this.profileImage.id = null;
                this.profileImage.fileName = null;
                this.profileImage.altText = null;
            },
        },
        mounted() {
            this.getAllData();
        },
        setup() {
            const refInputEl = ref(null)
            const previewEl = ref(null)

            const { inputImageRenderer } = useInputImageRenderer(refInputEl, previewEl)

            return {
                refInputEl,
                previewEl,
                inputImageRenderer,
            }
        },
    }
</script>

<style lang="scss">
    .image-thumb {
        width: 100px;
        height: 110px;
        -webkit-box-shadow: 0px 0px 3px 0px rgba(196,196,196,1);
        -moz-box-shadow: 0px 0px 3px 0px rgba(196,196,196,1);
        box-shadow: 0px 0px 3px 0px rgba(196,196,196,1);
        position: relative;
        border-radius: 5px;
    }

    .image-thumb img {
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

    #selectImage {
        position: absolute !important;
        top: -12px;
        right: -12px;
        padding: 5px !important;
    }

    #removeImage {
        position: absolute !important;
        bottom: -12px;
        right: -12px;
        padding: 5px !important;
    }
</style>