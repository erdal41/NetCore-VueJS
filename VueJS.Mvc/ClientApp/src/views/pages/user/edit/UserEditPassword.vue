<template>
    <b-card>
        <!-- form -->
        <b-form>
            <b-row>
                <b-col cols="12">
                    <div class="d-flex align-items-center mb-2">
                        <feather-icon icon="LockIcon"
                                      size="18" />
                        <h4 class="mb-0 ml-75">
                            Şifre Değiştir
                        </h4>
                    </div>
                </b-col>
                <!-- old password -->
                <b-col md="6">
                    <b-form-group label="Mevcut Şifreniz"
                                  label-for="Mevcut Şifreniz">
                        <b-input-group class="input-group-merge"
                                       :class="oldPasswordValidation ? 'is-valid' : 'is-invalid'">
                            <b-form-input id="account-old-password"
                                          v-model="oldPasswordValue"
                                          name="old-password"
                                          :type="passwordFieldTypeOld"
                                          :state="oldPasswordValidation"
                                          placeholder="Mevcut şifrenizi yazınız" />
                            <b-input-group-append is-text>
                                <feather-icon :icon="passwordToggleIconOld"
                                              class="cursor-pointer"
                                              @click="togglePasswordOld" />
                            </b-input-group-append>
                        </b-input-group>
                        <b-form-invalid-feedback :state="oldPasswordValidation">
                            Mevcut Şifre alanı boş olmamalıdır.
                        </b-form-invalid-feedback>
                    </b-form-group>
                </b-col>
                <!--/ old password -->
            </b-row>
            <b-row>
                <!-- new password -->
                <b-col md="6">
                    <b-form-group label-for="account-new-password"
                                  label="Yeni Şifreniz">
                        <b-input-group class="input-group-merge"
                                       :class="newPasswordValidation ? 'is-valid' : 'is-invalid'">
                            <b-form-input id="account-new-password"
                                          v-model="newPasswordValue"
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

                <b-col cols="12">
                    <hr class="my-2">
                </b-col>

                <!-- buttons -->
                <b-col cols="12">
                    <b-button v-ripple.400="'rgba(255, 255, 255, 0.15)'"
                              variant="primary"
                              class="mt-1 mr-1"
                              :disabled="disabledButton ? false : true"
                              @click.prevent="passwordChange">
                        Güncelle
                    </b-button>
                    <b-button v-ripple.400="'rgba(186, 191, 199, 0.15)'"
                              type="reset"
                              variant="outline-secondary"
                              class="mt-1">
                        Temizle
                    </b-button>
                </b-col>
                <!--/ buttons -->
            </b-row>
        </b-form>
    </b-card>
</template>

<script>
    import {
        BButton, BForm, BFormGroup, BFormInput, BRow, BCol, BCard, BInputGroup, BInputGroupAppend, BFormInvalidFeedback, BFormValidFeedback
    } from 'bootstrap-vue'
    import Ripple from 'vue-ripple-directive'
    import axios from 'axios';
    import ToastificationContent from '@core/components/toastification/ToastificationContent.vue';

    export default {
        components: {
            BButton,
            BForm,
            BFormGroup,
            BFormInput,
            BRow,
            BCol,
            BCard,
            BInputGroup,
            BInputGroupAppend,
            BFormInvalidFeedback,
            BFormValidFeedback,
        },
        directives: {
            Ripple,
        },
        data() {
            return {
                passwordErrorMessage: '',
                oldPasswordValue: '',
                newPasswordValue: '',
                RetypePassword: '',
                passwordFieldTypeOld: 'password',
                passwordFieldTypeNew: 'password',
                passwordFieldTypeRetype: 'password',
                passwordLength: 0,
                containsEightCharacters: false,
                containsNumber: false,
                containsLowercase: false,
                containsUppercase: false,
                containsSpecialCharacter: false,
                validPassword: false
            }
        },
        computed: {
            disabledButton() {
                if (this.oldPasswordValidation === true && this.newPasswordValidation === true && this.newPasswordRetypeValidation === true) {
                    return true;
                }
                else {
                    return false;
                }
            },
            oldPasswordValidation() {
                return this.oldPasswordValue.length > 0
            },
            newPasswordValidation() {
                this.passwordLength = this.newPasswordValue.length;
                const format = /[ !@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/;

                if (this.passwordLength > 8) {
                    this.containsEightCharacters = true;

                    this.containsNumber = /\d/.test(this.newPasswordValue);
                    this.containsLowercase = /[a-z]/.test(this.newPasswordValue);
                    this.containsUppercase = /[A-Z]/.test(this.newPasswordValue);
                    this.containsSpecialCharacter = format.test(this.newPasswordValue);

                    if (this.containsEightCharacters === true &&
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
                    this.containsEightCharacters = false;
                    this.validPassword = false;
                }                
                return this.validPassword;
            },
            newPasswordRetypeValidation() {
                return this.RetypePassword.length > 0 && this.newPasswordValue == this.RetypePassword;
            },
            passwordToggleIconOld() {
                return this.passwordFieldTypeOld === 'password' ? 'EyeIcon' : 'EyeOffIcon'
            },
            passwordToggleIconNew() {
                return this.passwordFieldTypeNew === 'password' ? 'EyeIcon' : 'EyeOffIcon'
            },
            passwordToggleIconRetype() {
                return this.passwordFieldTypeRetype === 'password' ? 'EyeIcon' : 'EyeOffIcon'
            },
        },
        methods: {
            togglePasswordOld() {
                this.passwordFieldTypeOld = this.passwordFieldTypeOld === 'password' ? 'text' : 'password'
            },
            togglePasswordNew() {
                this.passwordFieldTypeNew = this.passwordFieldTypeNew === 'password' ? 'text' : 'password'
            },
            togglePasswordRetype() {
                this.passwordFieldTypeRetype = this.passwordFieldTypeRetype === 'password' ? 'text' : 'password'
            },
            passwordChange() {
                if (this.oldPasswordValidation === true && this.newPasswordValidation === true && this.newPasswordRetypeValidation === true) {
                    axios.post('/admin/user-passwordchange',
                        {
                            CurrentPassword: this.oldPasswordValue,
                            NewPassword: this.newPasswordValue,
                            RepeatPassword: this.RetypePassword,
                        }).then((response) => {
                            console.log(response.data)
                            if (response.data.UserDto.ResultStatus === 0) {
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
                                this.$toast({
                                    component: ToastificationContent,
                                    props: {
                                        variant: 'danger',
                                        title: 'Başarısız İşlem!',
                                        icon: 'AlertOctagonIcon',
                                        text: response.data.UserDto.Message
                                    },
                                })
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
            }
        },
    }
</script>
