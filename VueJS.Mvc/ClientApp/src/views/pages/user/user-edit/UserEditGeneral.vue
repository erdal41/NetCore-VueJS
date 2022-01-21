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
                        <b-form-input v-model="optionsLocal.FirstName"
                                      name="firstname"
                                      placeholder="Ad" />
                    </b-form-group>
                </b-col>
                <b-col sm="6">
                    <b-form-group label="Soyad"
                                  label-for="Soyad">
                        <b-form-input v-model="optionsLocal.LastName"
                                      name="lastname"
                                      placeholder="Soyad" />
                    </b-form-group>
                </b-col>
                <b-col sm="6">
                    <b-form-group label="Kullanıcı Adı"
                                  label-for="username">
                        <validation-provider #default="{ errors }"
                                             name="username"
                                             rules="required">
                            <b-form-input id="username"
                                          v-model="optionsLocal.UserName"
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
                                              v-model="optionsLocal.Email"
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
                              class="mt-1 mr-1">
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
    import { required, email } from '@validations'

    import {
        BButton, BForm, BFormGroup, BFormInput, BInputGroup, BInputGroupPrepend, BRow, BCol, BCard, BCardText, BImg,
    } from 'bootstrap-vue'
    import Ripple from 'vue-ripple-directive'
    import ModalMedia from '@/views/pages/media/ModalMedia.vue';
    import { useInputImageRenderer } from '@core/comp-functions/forms/form-utils'
    import { ref } from '@vue/composition-api'

    extend('required', {
        ...required,
        message: 'Lütfen gerekli bilgileri yazınız.'
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
                optionsLocal: JSON.parse(JSON.stringify(this.generalData)),
                modalShow: Boolean,
                noImage: require('@/assets/images/default/default-post-image.jpg'),
                profileImage: {
                    id: null,
                    fileName: null,
                    altText: null,
                },
                required,
                email,
            }
        },
        methods: {
            resetForm() {
                this.optionsLocal = JSON.parse(JSON.stringify(this.generalData))

                if (this.optionsLocal.ProfileImage != null) {
                    this.profileImage.id = this.optionsLocal.ProfileImage.Id;
                    this.profileImage.fileName = this.optionsLocal.ProfileImage.FileName;
                    this.profileImage.altText = this.optionsLocal.ProfileImage.AltText;
                }
                else {
                    this.removeImage();
                }
            },
            profileImageLoad() {
                if (this.optionsLocal.ProfileImage != null) {
                    this.profileImage.id = this.optionsLocal.ProfileImage.Id;
                    this.profileImage.fileName = this.optionsLocal.ProfileImage.FileName;
                    this.profileImage.altText = this.optionsLocal.ProfileImage.AltText;
                }
            },
            imageChange(id, name, altText) {
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
            this.profileImageLoad();
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