<template>
    <b-row>
        <b-col cols="12">
            <modal-media v-bind:show="modalShow"
                         @changeImage="imageChange"
                         ref="modalMedia"></modal-media>
            <form-wizard color="#7367F0"
                         :title="null"
                         :subtitle="null"
                         layout="vertical"
                         finish-button-text="Kaydet"
                         back-button-text="Önceki"
                         next-button-text="Sonraki"
                         class="vertical-steps steps-transparent mb-3"
                         @on-complete="formSubmitted">

                <!-- account details tab -->
                <tab-content title="Kullanıcı Bilgileri"
                             icon="feather icon-file-text">
                    <b-row>
                        <b-col cols="12"
                               class="mb-2">
                            <h5 class="mb-0">
                                Kullanıcı Bilgileri
                            </h5>
                            <small class="text-muted">
                                Gerekli kullanıcı bilgilerini giriniz.
                            </small>
                        </b-col>
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
                        <!-- username -->
                        <b-col md="6">
                            <b-form-group label-for="account-new-password"
                                          label="Yalnızca harfler, sayılar veya alt çizgilerden oluşabilir.">
                                <b-input-group class="input-group-merge"
                                               :class="userNameValidation ? 'is-valid' : 'is-invalid'">
                                    <b-input-group-prepend is-text>
                                        <feather-icon icon="UserIcon" />
                                    </b-input-group-prepend>
                                    <b-form-input id="account-username"
                                                  v-model="userAddDto.UserName"
                                                  type="text"
                                                  :state="userNameValidation"
                                                  name="username"
                                                  placeholder="Kullanıcı Adı" />
                                </b-input-group>
                                <b-form-invalid-feedback :state="userNameValidation">
                                    Kullanıcı adınız harfler, sayılar veya alt çizgilerden oluşabilir.
                                </b-form-invalid-feedback>
                                <b-form-valid-feedback :state="userNameValidation">
                                    İyi görünüyor.
                                </b-form-valid-feedback>
                            </b-form-group>
                        </b-col>
                        <!--/ username -->
                        <!-- email -->
                        <b-col md="6">
                            <b-form-group label-for="account-email"
                                          label="E-Posta Adresi">
                                <b-input-group class="input-group-merge"
                                               :class="emailValidation ? 'is-valid' : 'is-invalid'">
                                    <b-input-group-prepend is-text>
                                        <feather-icon icon="MailIcon" />
                                    </b-input-group-prepend>
                                    <b-form-input id="account-email"
                                                  v-model="userAddDto.Email"
                                                  type="text"
                                                  :state="emailValidation"
                                                  name="username"
                                                  placeholder="E-Posta Adresi" />
                                </b-input-group>
                                <b-form-invalid-feedback :state="emailValidation">
                                    Lütfen geçerli bir e-posta adresi giriniz.
                                </b-form-invalid-feedback>
                                <b-form-valid-feedback :state="emailValidation">
                                    İyi görünüyor.
                                </b-form-valid-feedback>
                            </b-form-group>
                        </b-col>
                        <!--/ email -->
                        <!-- new password -->
                        <b-col md="6">
                            <b-form-group label-for="account-new-password"
                                          label="Yeni Şifreniz">
                                <b-input-group class="input-group-merge"
                                               :class="newPasswordValidation ? 'is-valid' : 'is-invalid'">
                                    <b-form-input id="account-new-password"
                                                  v-model="userAddDto.Password"
                                                  :type="passwordFieldTypeNew"
                                                  :state="newPasswordValidation"
                                                  name="new-password"
                                                  placeholder="Yeni şifrenizi yazınız" />
                                    <b-input-group-append is-text>
                                        <feather-icon :icon="passwordToggleIconNew"
                                                      class="cursor-pointer"
                                                      @click="togglePasswordNew" />
                                    </b-input-group-append>
                                </b-input-group>
                                <b-form-invalid-feedback :state="newPasswordValidation">
                                    Şifreniz en az bir büyük harf, bir küçük harf, bir özel karakter ve bir rakam içermelidir.
                                </b-form-invalid-feedback>
                                <b-form-valid-feedback :state="newPasswordValidation">
                                    İyi görünüyor.
                                </b-form-valid-feedback>
                            </b-form-group>
                        </b-col>
                        <!--/ new password -->
                        <!-- retype password -->
                        <b-col md="6">
                            <b-form-group label-for="account-retype-new-password"
                                          label="Tekrar Yeni Şifreniz">
                                <b-input-group class="input-group-merge"
                                               :class="newPasswordRetypeValidation ? 'is-valid' : 'is-invalid'">
                                    <b-form-input id="account-retype-new-password"
                                                  v-model="RetypePassword"
                                                  :type="passwordFieldTypeRetype"
                                                  :state="newPasswordRetypeValidation"
                                                  name="retype-password"
                                                  placeholder="Yeni şifrenizi tekrar yazınız" />
                                    <b-input-group-append is-text>
                                        <feather-icon :icon="passwordToggleIconRetype"
                                                      class="cursor-pointer"
                                                      @click="togglePasswordRetype" />
                                    </b-input-group-append>
                                </b-input-group>
                                <b-form-invalid-feedback :state="newPasswordRetypeValidation">
                                    Girmiş olduğunuz yeni şifreler uyuşmuyor.
                                </b-form-invalid-feedback>
                                <b-form-valid-feedback :state="newPasswordRetypeValidation">
                                    İyi görünüyor.
                                </b-form-valid-feedback>
                            </b-form-group>
                        </b-col>
                        <!--/ retype password -->
                    </b-row>
                </tab-content>

                <!-- personal details tab -->
                <tab-content title="Kişisel Bilgiler"
                             icon="feather icon-user">
                    <b-row>
                        <b-col cols="12"
                               class="mb-2">
                            <h5 class="mb-0">
                                Kişisel Bilgiler
                            </h5>
                            <small class="text-muted">Kişisel bilgilerinizi giriniz.</small>
                        </b-col>
                        <b-col sm="6">
                            <b-form-group label="Ad"
                                          label-for="firstname">
                                <b-input-group>
                                    <b-input-group-prepend is-text>
                                        <feather-icon size="16"
                                                      icon="UserIcon" />
                                    </b-input-group-prepend>
                                    <b-form-input v-model="userAddDto.FirstName"
                                                  name="firstname"
                                                  placeholder="Ad" />
                                </b-input-group>
                            </b-form-group>
                        </b-col>
                        <b-col sm="6">
                            <b-form-group label="Soyad"
                                          label-for="Soyad">
                                <b-input-group>
                                    <b-input-group-prepend is-text>
                                        <feather-icon size="16"
                                                      icon="UserIcon" />
                                    </b-input-group-prepend>
                                    <b-form-input v-model="userAddDto.LastName"
                                                  name="lastname"
                                                  placeholder="Soyad" />
                                </b-input-group>
                            </b-form-group>
                        </b-col>
                        <!-- bio -->
                        <b-col cols="12">
                            <b-form-group label="Biyografi"
                                          label-for="bio-area">
                                <b-input-group>
                                    <b-input-group-prepend is-text>
                                        <feather-icon size="16"
                                                      icon="AlignJustifyIcon" />
                                    </b-input-group-prepend>
                                    <b-form-textarea id="bio-area"
                                                     v-model="userAddDto.About"
                                                     rows="4"
                                                     placeholder="Biyografi bilgilerinizi buraya girebilirsiniz..." />
                                </b-input-group>
                            </b-form-group>
                        </b-col>
                        <!--/ bio -->
                        <!-- website -->
                        <b-col md="6">
                            <b-form-group label-for="website"
                                          label="Website">
                                <b-input-group>
                                    <b-input-group-prepend is-text>
                                        <feather-icon size="16"
                                                      icon="GlobeIcon" />
                                    </b-input-group-prepend>
                                    <b-form-input id="website"
                                                  v-model="userAddDto.WebsiteLink"
                                                  placeholder="Website adresiniz" />
                                </b-input-group>
                            </b-form-group>
                        </b-col>
                        <!--/ website -->
                        <!-- phone -->
                        <b-col md="6">
                            <b-form-group label-for="phone"
                                          label="Telefon">
                                <b-input-group>
                                    <b-input-group-prepend is-text>
                                        +90
                                    </b-input-group-prepend>
                                    <cleave id="phone"
                                            v-model="userAddDto.PhoneNumber"
                                            class="form-control"
                                            :raw="false"
                                            :options="maskOptions.phone"
                                            placeholder="123 123 12 12" />
                                </b-input-group>
                            </b-form-group>
                        </b-col>
                        <!-- phone -->
                    </b-row>
                </tab-content>

                <!-- social link -->
                <tab-content title="Sosyal Medya"
                             icon="feather icon-link">
                    <b-row>
                        <b-col cols="12"
                               class="mb-2">
                            <h5 class="mb-0">
                                Sosyal Medya
                            </h5>
                            <small class="text-muted">Sosyal medya profil linklerini giriniz.</small>
                        </b-col>
                        <b-col md="6">
                            <b-form-group label="Facebook"
                                          label-for="facebook">
                                <b-input-group class="input-group-merge">
                                    <b-input-group-prepend is-text>
                                        <feather-icon size="16"
                                                      icon="FacebookIcon" />
                                    </b-input-group-prepend>
                                    <b-form-input id="facebook"
                                                  v-model="userAddDto.FacebookLink"
                                                  type="url"
                                                  placeholder="Facebook profil linki" />
                                </b-input-group>
                            </b-form-group>
                        </b-col>
                        <b-col md="6">
                            <b-form-group label="Twitter"
                                          label-for="twitter">
                                <b-input-group class="input-group-merge">
                                    <b-input-group-prepend is-text>
                                        <feather-icon size="16"
                                                      icon="TwitterIcon" />
                                    </b-input-group-prepend>
                                    <b-form-input id="twitter"
                                                  v-model="userAddDto.TwitterLink"
                                                  type="url"
                                                  placeholder="Twitter profil linki" />
                                </b-input-group>
                            </b-form-group>
                        </b-col>
                        <b-col md="6">
                            <b-form-group label="Instagram"
                                          label-for="instagram">
                                <b-input-group class="input-group-merge">
                                    <b-input-group-prepend is-text>
                                        <feather-icon size="16"
                                                      icon="InstagramIcon" />
                                    </b-input-group-prepend>
                                    <b-form-input id="instagram"
                                                  v-model="userAddDto.InstagramLink"
                                                  type="url"
                                                  placeholder="Instagram profil linki" />
                                </b-input-group>
                            </b-form-group>
                        </b-col>
                        <b-col md="6">
                            <b-form-group label="LinkedIn"
                                          label-for="linkedin">
                                <b-input-group class="input-group-merge">
                                    <b-input-group-prepend is-text>
                                        <feather-icon size="16"
                                                      icon="LinkedinIcon" />
                                    </b-input-group-prepend>
                                    <b-form-input id="linkedin"
                                                  v-model="userAddDto.LinkedInLink"
                                                  type="url"
                                                  placeholder="LinkedIn profil linki" />
                                </b-input-group>
                            </b-form-group>
                        </b-col>
                        <b-col md="6">
                            <b-form-group label="Youtube"
                                          label-for="youtube">
                                <b-input-group class="input-group-merge">
                                    <b-input-group-prepend is-text>
                                        <feather-icon size="16"
                                                      icon="YoutubeIcon" />
                                    </b-input-group-prepend>
                                    <b-form-input id="youtube"
                                                  v-model="userAddDto.YoutubeLink"
                                                  type="url"
                                                  placeholder="Youtube kanal linki" />
                                </b-input-group>
                            </b-form-group>
                        </b-col>
                        <b-col md="6">
                            <b-form-group label="GitHub"
                                          label-for="github">
                                <b-input-group class="input-group-merge">
                                    <b-input-group-prepend is-text>
                                        <feather-icon size="16"
                                                      icon="GithubIcon" />
                                    </b-input-group-prepend>
                                    <b-form-input id="github"
                                                  v-model="userAddDto.GitHubLink"
                                                  type="url"
                                                  placeholder="GitHub profil linki" />
                                </b-input-group>
                            </b-form-group>
                        </b-col>
                    </b-row>
                </tab-content>

                <b-button slot="prev"
                          variant="secondary"
                          class="mt-1">
                    <feather-icon icon="ChevronLeftIcon" />
                    Önceki
                </b-button>
                <b-button slot="next"
                          variant="primary"
                          class="mt-1"
                          :disabled="disabledButton ? false : true">
                    Sonraki
                    <feather-icon icon="ChevronRightIcon" />
                </b-button>
                <b-button slot="finish"
                          variant="primary"
                          class="mt-1">Kaydet</b-button>
            </form-wizard>
        </b-col>
    </b-row>
</template>

<script>
    import { FormWizard, TabContent } from 'vue-form-wizard'
    import 'vue-form-wizard/dist/vue-form-wizard.min.css'
    import {
        BRow, BCol, BButton, BFormGroup, BFormInput, BFormTextarea, BImg, BInputGroup, BInputGroupPrepend, BInputGroupAppend, BFormInvalidFeedback, BFormValidFeedback
    } from 'bootstrap-vue'
    import ToastificationContent from '@core/components/toastification/ToastificationContent.vue'
    import Cleave from 'vue-cleave-component'
    import 'cleave.js/dist/addons/cleave-phone.tr'
    import Ripple from 'vue-ripple-directive'
    import axios from 'axios';
    import ModalMedia from '@/views/pages/media/ModalMedia.vue';

    export default {
        components: {
            ModalMedia,
            FormWizard,
            TabContent,
            BRow,
            BCol,
            BButton,
            BFormGroup,
            BFormInput,
            BFormTextarea,
            BImg,
            BInputGroup,
            BInputGroupPrepend,
            BInputGroupAppend,
            BFormInvalidFeedback,
            BFormValidFeedback,
            ToastificationContent,
            Cleave
        },
        directives: {
            Ripple,
        },
        data() {
            return {
                modalShow: Boolean,
                noImage: require('@/assets/images/default/default-post-image.jpg'),
                profileImage: {
                    id: null,
                    fileName: null,
                    altText: null,
                },
                userNameLength: 0,
                userNameCharacterLength: false,
                userNameSpecialFormat: false,
                validUserName: false,
                maskOptions: {
                    phone: {
                        phone: true,
                        phoneRegionCode: 'TR',
                    },
                },
                RetypePassword: '',
                passwordFieldTypeNew: 'password',
                passwordFieldTypeRetype: 'password',
                passwordLength: 0,
                passwordCharacterLength: false,
                containsNumber: false,
                containsLowercase: false,
                containsUppercase: false,
                containsSpecialCharacter: false,
                validPassword: false,
                userAddDto: {
                    ProfileImageId: '',
                    UserName: '',
                    Email: '',
                    Password: '',
                    RetypePassword: '',
                    FirstName: '',
                    LastName: '',
                    About: '',
                    WebsiteLink: '',
                    PhoneNumber: '',
                    FacebookLink: '',
                    TwitterLink: '',
                    InstagramLink: '',
                    LinkedInLink: '',
                    YoutubeLink: '',
                    GitHubLink: '',
                }
            }
        },
        computed: {
            disabledButton() {
                if (this.emailValidation === true && this.userNameValidation === true && this.newPasswordValidation === true && this.newPasswordRetypeValidation === true) {
                    return true;
                }
                else {
                    return false;
                }
            },
            emailValidation() {
                if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(this.userAddDto.Email)) {
                    return true;
                } else {
                    return false;
                }
            },
            userNameValidation() {
                this.userNameLength = this.userAddDto.UserName.length;
                const format = /^[a-zA-Z0-9_]*$/;

                if (this.userNameLength >= 6 && this.userNameLength <= 30) {
                    this.userNameCharacterLength = true;
                } else {
                    this.userNameCharacterLength = false;
                }
                this.userNameSpecialFormat = format.test(this.userAddDto.UserName);

                if (this.userNameCharacterLength === true &&
                    this.userNameSpecialFormat === true) {
                    this.validUserName = true;
                } else {
                    this.validUserName = false;
                }
                return this.validUserName;
            },
            newPasswordValidation() {
                this.passwordLength = this.userAddDto.Password.length;
                const format = /[ !@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/;

                if (this.passwordLength > 8) {
                    this.passwordCharacterLength = true;
                } else {
                    this.passwordCharacterLength = false;
                }
                this.containsNumber = /\d/.test(this.userAddDto.Password);
                this.containsLowercase = /[a-z]/.test(this.userAddDto.Password);
                this.containsUppercase = /[A-Z]/.test(this.userAddDto.Password);
                this.containsSpecialCharacter = format.test(this.userAddDto.Password);

                if (this.passwordCharacterLength === true &&
                    this.containsSpecialCharacter === true &&
                    this.containsLowercase === true &&
                    this.containsUppercase === true &&
                    this.containsNumber === true) {
                    this.validPassword = true;
                } else {
                    this.validPassword = false;
                }
                return this.validPassword;
            },
            newPasswordRetypeValidation() {
                return this.RetypePassword.length > 0 && this.userAddDto.Password == this.RetypePassword;
            },
            passwordToggleIconNew() {
                return this.passwordFieldTypeNew === 'password' ? 'EyeIcon' : 'EyeOffIcon'
            },
            passwordToggleIconRetype() {
                return this.passwordFieldTypeRetype === 'password' ? 'EyeIcon' : 'EyeOffIcon'
            },
        },
        methods: {
            togglePasswordNew() {
                this.passwordFieldTypeNew = this.passwordFieldTypeNew === 'password' ? 'text' : 'password'
            },
            togglePasswordRetype() {
                this.passwordFieldTypeRetype = this.passwordFieldTypeRetype === 'password' ? 'text' : 'password'
            },
            imageChange(id, name, altText) {
                this.profileImage.id = id;
                this.profileImage.fileName = name;
                this.profileImage.altText = altText;
                console.log(id)
            },
            selectImage: function (e) {
                this.modalShow = true;
            },
            removeImage() {
                this.profileImage.id = null;
                this.profileImage.fileName = null;
                this.profileImage.altText = null;
            },
            nextButton() {
                this.$refs.userAddForm.validate().then(success => {
                    if (success) {
                        console.log(success)
                    }
                    else {
                        console.log(success)
                    }
                });
            },
            formSubmitted() {
                this.userAddDto.ProfileImageId = this.profileImage.id;
                axios.post('/admin/user-new',
                    {
                        UserAddDto: this.userAddDto
                    }).then((response) => {
                        if (response.data.UserDto.ResultStatus === 0) {
                            this.$router.push({ name: 'pages-user-edit', query: { edit: response.data.UserDto.User.Id } });
                            this.$toast({
                                component: ToastificationContent,
                                props: {
                                    variant: 'success',
                                    title: 'Başarılı İşlem!',
                                    icon: 'CheckIcon',
                                    text: response.data.UserDto.Message
                                }
                            })
                        }
                        else {
                            const messages= response.data.UserDto.Message.split('*')
                            messages.forEach(message => {
                                if (message != "") {
                                    this.$toast({
                                        component: ToastificationContent,
                                        props: {
                                            variant: 'danger',
                                            title: 'Başarısız İşlem!',
                                            icon: 'AlertOctagonIcon',
                                            text: message,
                                        },
                                    })
                                }
                            });
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
                                text: 'Hata oluştu. Lütfen tekrar deneyin.',
                            },
                        })
                    });
            }
        },
    }
</script>

<style lang="scss">
    @import '@core/scss/vue/libs/vue-wizard.scss';

    .vertical-steps.vue-form-wizard .wizard-card-footer {
        margin-left: 230px !important;
    }

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