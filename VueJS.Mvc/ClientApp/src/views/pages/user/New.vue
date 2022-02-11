<template>
    <b-row>
        <b-col class="content-header-left mb-2"
               cols="12"
               md="8">
            <b-row class="breadcrumbs-top">
                <b-col cols="12">
                    <h2 class="content-header-title float-left pr-1 mb-0">
                        Kullanıcı Ekle
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
                             icon="feather icon-file-text"
                             tabindex="0">
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
                                                  placeholder="Kullanıcı Adı" 
                                                  @input="userNameTextChange"/>
                                </b-input-group>
                                <b-form-invalid-feedback :state="userNameValidation">
                                    {{ userErrorMessage }}
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
                                                  placeholder="E-Posta Adresi" 
                                                  @input="emailTextChange"/>
                                </b-input-group>
                                <b-form-invalid-feedback :state="emailValidation">
                                    {{ emailErrorMessage }}
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
                                    {{ passwordErrorMessage }}
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
                             icon="feather icon-user"
                             tabindex="1"
                             :disabled="disabled">
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
                             icon="feather icon-link"
                             tabindex="2">
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

                <!-- user role -->
                <tab-content title="Roller"
                             icon="feather icon-shield"
                             tabindex="3">
                    <b-row>
                        <b-col cols="12"
                               class="mb-2">
                            <h5 class="mb-0">
                                Roller
                            </h5>
                            <small class="text-muted">Kullanıcıya atanacak rolleri seçiniz.</small>
                        </b-col>
                        <b-col cols="12">
                            <b-table striped
                                     responsive
                                     class="mb-0"
                                     :fields="fields"
                                     :items="roles"
                                     ref="roleTable">
                                <template #head(read)="head">
                                    <b-form-checkbox v-model="selectMultiReadCheck"
                                                     :value="true"
                                                     @change="selectAllReadRows">
                                        {{head.label}}
                                    </b-form-checkbox>
                                </template>
                                <template #head(create)="head">
                                    <b-form-checkbox v-model="selectMultiCreateCheck"
                                                     :value="true"
                                                     @change="selectAllCreateRows">
                                        {{head.label}}
                                    </b-form-checkbox>
                                </template>
                                <template #head(update)="head">
                                    <b-form-checkbox v-model="selectMultiUpdateCheck"
                                                     :value="true"
                                                     @change="selectAllUpdateRows">
                                        {{head.label}}
                                    </b-form-checkbox>
                                </template>
                                <template #head(delete)="head">
                                    <b-form-checkbox v-model="selectMultiDeleteCheck"
                                                     :value="true"
                                                     @change="selectAllDeleteRows">
                                        {{head.label}}
                                    </b-form-checkbox>
                                </template>

                                <template #cell(module)="data">
                                    {{ data.item.module }}
                                </template>
                                <template #cell(read)="data">
                                    <b-form-checkbox v-show="data.value.show"
                                                     v-model="checkedRoleRead"
                                                     :value="data.value.action"
                                                     @change="changeReadRow" />
                                </template>
                                <template #cell(create)="data">
                                    <b-form-checkbox v-show="data.value.show"
                                                     v-model="checkedRoleCreate"
                                                     :value="data.value.action"
                                                     @change="changeCreateRow" />
                                </template>
                                <template #cell(update)="data">
                                    <b-form-checkbox v-show="data.value.show"
                                                     v-model="checkedRoleUpdate"
                                                     :value="data.value.action"
                                                     @change="changeUpdateRow" />
                                </template>
                                <template #cell(delete)="data">
                                    <b-form-checkbox v-show="data.value.show"
                                                     v-model="checkedRoleDelete"
                                                     :value="data.value.action"
                                                     @change="changeDeleteRow" />
                                </template>
                            </b-table>
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
        BRow, BCol, BBreadcrumb, BBreadcrumbItem, BButton, BTable, BFormGroup, BFormInput, BFormCheckbox, BFormTextarea, BImg, BInputGroup, BInputGroupPrepend, BInputGroupAppend, BFormInvalidFeedback, BFormValidFeedback
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
            BBreadcrumb,
            BBreadcrumbItem,
            BTable,
            BButton,
            BFormGroup,
            BFormInput,
            BFormCheckbox,
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
                breadcrumbs: [
                    {
                        text: 'Kullanıcılar',
                        to: { name: 'pages-user-list' },
                    },
                    {
                        text: 'Kullanıcı Ekle',
                        active: true,
                    }
                ],
                userErrorMessage: '',
                emailErrorMessage: '',
                passwordErrorMessage: '',
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
                validEmail: false,
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
                },
                defaultRoles: [],
                roles: [],
                userRoles: [],
                fields: [
                    { key: 'Module', label: 'Modül', sortable: false },
                    { key: 'read', label: 'Okuma', sortable: false },
                    { key: 'create', label: 'Oluşturma', sortable: false },
                    { key: 'update', label: 'Güncelleme', sortable: false },
                    { key: 'delete', label: 'SİLME', sortable: false }],
                moduleList: [
                    'Dashboard',
                    'Basepage',
                    'Otherpage',
                    'Article',
                    'Category',
                    'Tag',
                    'User',
                    'Role',
                    'Comment',
                    'Urlredirect',
                    'Seo'
                ],
                selectMultiReadCheck: false,
                selectMultiCreateCheck: false,
                selectMultiUpdateCheck: false,
                selectMultiDeleteCheck: false,
                checkedRoleRead: [],
                checkedRoleCreate: [],
                checkedRoleUpdate: [],
                checkedRoleDelete: [],
                errorMessages: [],
                disabled: ''
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
                if (this.emailErrorMessage.includes('e-posta adresi zaten kayıtlı')) {
                    this.validEmail = false;
                } else {
                    this.emailErrorMessage = 'Lütfen geçerli bir e-posta adresi giriniz.';

                    if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(this.userAddDto.Email)) {
                        this.validEmail = true;
                    } else {
                        this.validEmail = false;
                    }
                }
                return this.validEmail;
            },
            userNameValidation() {
                if (this.userErrorMessage.includes('kullanıcı adı zaten kayıtlı')) {
                    this.validUserName = false;
                }
                else {
                    this.userNameLength = this.userAddDto.UserName.length;
                    const format = /^[a-zA-Z0-9_]*$/;

                    if (this.userNameLength >= 6 && this.userNameLength <= 30) {
                        this.userNameCharacterLength = true;

                        this.userNameSpecialFormat = format.test(this.userAddDto.UserName);

                        if (this.userNameCharacterLength === true &&
                            this.userNameSpecialFormat === true) {
                            this.validUserName = true;
                        } else {
                            this.userErrorMessage = 'Kullanıcı adınız harfler, sayılar veya alt çizgilerden oluşabilir.';
                            this.validUserName = false;
                        }
                    } else {
                        this.userErrorMessage = 'Kullanıcı adınız minimum 6, maksimum 30 karakter olmalıdır.';
                        this.validUserName = false;
                    }                    
                }
                return this.validUserName;
            },
            newPasswordValidation() {
                this.passwordLength = this.userAddDto.Password.length;
                const format = /[ !@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/;

                if (this.passwordLength > 8) {
                    this.passwordCharacterLength = true;

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
                        this.passwordErrorMessage = 'Şifreniz en az bir büyük harf, bir küçük harf, bir özel karakter ve bir rakam içermelidir.';
                        this.validPassword = false;
                    }
                } else {
                    this.passwordErrorMessage = 'Şifreniz minimum 8, maksimum 30 karakter olmalıdır.';
                    this.passwordCharacterLength = false;
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
            userNameTextChange() {
                this.userErrorMessage = '';
            },
            emailTextChange() {
                this.emailErrorMessage = '';
            },
            getRoles() {
                this.$http.get('/admin/user-allroles').then(response => {
                    console.log(response.data.Roles)
                    this.defaultRoles = response.data.Roles;
                    this.moduleList.forEach(module => {
                        if (module === 'Dashboard') {
                            var rolesSplit = response.data.Roles.filter(role => role.Name.includes('Dashboard'));
                            this.roles.push(
                                {
                                    module: "Gösterge Paneli",
                                    read: {
                                        action: module + '.read',
                                        show: rolesSplit.filter(x => x.Name.includes('read')) ? true : false
                                    },
                                    create: {
                                        action: false,
                                        show: false
                                    },
                                    update: {
                                        action: false,
                                        show: false
                                    },
                                    delete: {
                                        action: false,
                                        show: false
                                    },
                                });
                        } else if (module === 'User') {
                            var rolesSplit = response.data.Roles.filter(role => role.Name.includes('User'));
                            this.roles.push(
                                {
                                    module: "Kullanıcılar",
                                    read: {
                                        action: false,
                                        show: false
                                    },
                                    create: {
                                        action: module + '.create',
                                        show: rolesSplit.filter(x => x.Name.includes('create')) ? true : false
                                    },
                                    update: {
                                        action: module + '.update',
                                        show: rolesSplit.filter(x => x.Name.includes('update')) ? true : false
                                    },
                                    delete: {
                                        action: module + '.delete',
                                        show: rolesSplit.filter(x => x.Name.includes('delete')) ? true : false
                                    },
                                });
                        } else if (module === 'Role') {
                            var rolesSplit = response.data.Roles.filter(role => role.Name.includes('Role'));
                            this.roles.push(
                                {
                                    module: "Roller",
                                    read: {
                                        action: false,
                                        show: false
                                    },
                                    create: {
                                        action: module + '.create',
                                        show: rolesSplit.filter(x => x.Name.includes('create')) ? true : false
                                    },
                                    update: {
                                        action: module + '.update',
                                        show: rolesSplit.filter(x => x.Name.includes('update')) ? true : false
                                    },
                                    delete: {
                                        action: false,
                                        show: false
                                    },
                                });
                        } else if (module === 'Seo') {
                            var rolesSplit = response.data.Roles.filter(role => role.Name.includes('Seo'));
                            this.roles.push(
                                {
                                    module: "Seo Ayarları",
                                    read: {
                                        action: false,
                                        show: false
                                    },
                                    create: {
                                        action: module + '.create',
                                        show: rolesSplit.filter(x => x.Name.includes('create')) ? true : false
                                    },
                                    update: {
                                        action: module + '.update',
                                        show: rolesSplit.filter(x => x.Name.includes('update')) ? true : false
                                    },
                                    delete: {
                                        action: false,
                                        show: false
                                    },
                                });
                        } else {
                            var rolesSplit = response.data.Roles.filter(role => role.Name.includes(module));
                            this.roles.push(
                                {
                                    module:
                                        module == 'Category' ? 'Kategori' :
                                            module == 'Basepage' ? 'Temel Sayfalar' :
                                                module == 'Otherpage' ? 'Sayfalar' :
                                                    module == 'Article' ? 'Makaleler' :
                                                        module == 'Category' ? 'Kategoriler' :
                                                            module == 'Tag' ? 'Etiketler' :
                                                                module == 'Role' ? 'Roller' :
                                                                    module == 'Comment' ? 'Yorumlar' :
                                                                        module == 'Urlredirect' ? 'Link Yönlendirme' :
                                                                            null,
                                    read: {
                                        action: module + '.read',
                                        show: rolesSplit.filter(x => x.Name.includes('read')) ? true : false
                                    },
                                    create: {
                                        action: module + '.create',
                                        show: true
                                    },
                                    update: {
                                        action: module + '.update',
                                        show: true
                                    },
                                    delete: {
                                        action: module + '.delete',
                                        show: true
                                    }
                                });
                        }
                    });

                    this.changeReadRow();
                    this.changeCreateRow();
                    this.changeUpdateRow();
                    this.changeDeleteRow();
                }).catch((error) => {
                    console.log(error)
                    console.log(error.request)
                })
            },
            changeReadRow() {
                if (this.checkedRoleRead.length == 8) {
                    this.selectMultiReadCheck = 'true';
                }
                else {
                    this.selectMultiReadCheck = 'false';
                }
            },
            changeCreateRow() {
                if (this.checkedRoleCreate.length == 10) {
                    this.selectMultiCreateCheck = 'true';
                }
                else {
                    this.selectMultiCreateCheck = 'false';
                }
            },
            changeUpdateRow() {
                if (this.checkedRoleUpdate.length == 10) {
                    this.selectMultiUpdateCheck = 'true';
                }
                else {
                    this.selectMultiUpdateCheck = 'false';
                }
            },
            changeDeleteRow() {
                if (this.checkedRoleDelete.length == 8) {
                    this.selectMultiDeleteCheck = 'true';
                }
                else {
                    this.selectMultiDeleteCheck = 'false';
                }
            },
            selectAllReadRows(value) {
                this.checkedRoleRead = [];
                if (value) {
                    var roleReads = this.defaultRoles.filter(role => role.Name.includes('read'));
                    roleReads.forEach(role => {
                        this.checkedRoleRead.push(role.Name);
                    })
                }
                else {
                    this.checkedRoleRead = [];
                }
            },
            selectAllCreateRows(value) {
                this.checkedRoleCreate = [];
                if (value) {
                    var roleCreates = this.defaultRoles.filter(role => role.Name.includes('create'));
                    roleCreates.forEach(role => {
                        this.checkedRoleCreate.push(role.Name);
                    })
                }
                else {
                    this.checkedRoleCreate = [];
                }
            },
            selectAllUpdateRows(value) {
                this.checkedRoleUpdate = [];
                if (value) {
                    var roleUpdates = this.defaultRoles.filter(role => role.Name.includes('update'));
                    roleUpdates.forEach(role => {
                        this.checkedRoleUpdate.push(role.Name);
                    })
                }
                else {
                    this.checkedRoleUpdate = [];
                }
            },
            selectAllDeleteRows(value) {
                this.checkedRoleDelete = [];
                if (value) {
                    var roleDeletes = this.defaultRoles.filter(role => role.Name.includes('delete'));
                    roleDeletes.forEach(role => {
                        this.checkedRoleDelete.push(role.Name);
                    })
                }
                else {
                    this.checkedRoleDelete = [];
                }
            },
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
                this.userRoles = this.checkedRoleRead.concat(this.checkedRoleCreate.concat(this.checkedRoleUpdate.concat(this.checkedRoleDelete)));
                console.log(this.userRoles);
                if (this.userRoles.length <= 0) {
                    this.$swal({
                        title: 'Uyarı!',
                        text: "Kullanıcıya hiçbir rol atanmadı. Yine de kaydetmek istiyor musunuz?",
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
                            this.saveUser();
                        }
                    })
                } else {
                    this.saveUser();
                }
            },
            saveUser() {
                this.userAddDto.ProfileImageId = this.profileImage.id;
                axios.post('/admin/user-new',
                    {
                        UserAddDto: this.userAddDto
                    })
                    .then((responseUser) => {
                        if (responseUser.data.UserDto.ResultStatus === 0) {

                            axios.post('/admin/user-roleassign',
                                {
                                    UserId: responseUser.data.UserDto.User.Id,
                                    UserRoles: this.userRoles

                                }).then((responseRole) => {
                                    console.log(responseRole.data)
                                    if (responseRole.data.UserDto.ResultStatus === 0) {
                                        this.$router.push({ name: 'pages-user-edit', query: { edit: responseUser.data.UserDto.User.Id } });
                                        this.$toast({
                                            component: ToastificationContent,
                                            props: {
                                                variant: 'success',
                                                title: 'Başarılı İşlem!',
                                                icon: 'CheckIcon',
                                                text: responseUser.data.UserDto.Message
                                            }
                                        })
                                    }
                                })

                        }
                        else {
                            var messages = responseUser.data.UserDto.Message.split('*')
                            messages.forEach(message => {
                                if (message != "") {

                                    if (message.includes('kullanıcı')) {
                                        this.userErrorMessage = message;
                                    }

                                    if (message.includes('posta')) {
                                        this.emailErrorMessage = message;
                                    }

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
        mounted() {
            this.getRoles();
        }
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