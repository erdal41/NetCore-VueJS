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
                                          v-model="passwordValueOld"
                                          name="old-password"
                                          :type="passwordFieldTypeOld"
                                          :state="oldPasswordValidation"
                                          placeholder="Mevcut şifrenizi yazınız"
                                          @keyup.space="preventLeadingSpace" />
                            <b-input-group-append is-text>
                                <feather-icon :icon="passwordToggleIconOld"
                                              class="cursor-pointer"
                                              @click="togglePasswordOld" />
                            </b-input-group-append>
                        </b-input-group>
                        <b-form-invalid-feedback :state="oldPasswordValidation">
                            Lütfen mevcut şifrenizi giriniz.
                        </b-form-invalid-feedback>
                        <b-form-valid-feedback :state="oldPasswordValidation">
                            Looks Good.
                        </b-form-valid-feedback>
                    </b-form-group>
                </b-col>
                <!--/ old password -->
            </b-row>
            <b-row>
                <!-- new password -->
                <b-col md="6">
                    <b-form-group label-for="Yeni Şifreniz"
                                  label="Yeni Şifreniz">
                        <b-input-group class="input-group-merge">
                            <b-form-input id="account-new-password"
                                          v-model="newPasswordValue"
                                          :type="passwordFieldTypeNew"
                                          name="new-password"
                                          placeholder="Yeni şifrenizi yazınız" />
                            <b-input-group-append is-text>
                                <feather-icon :icon="passwordToggleIconNew"
                                              class="cursor-pointer"
                                              @click="togglePasswordNew" />
                            </b-input-group-append>
                        </b-input-group>
                    </b-form-group>
                </b-col>
                <!--/ new password -->
                <!-- retype password -->
                <b-col md="6">
                    <b-form-group label-for="Yeni Şifreniz"
                                  label="Tekrar Yeni Şifreniz">
                        <b-input-group class="input-group-merge">
                            <b-form-input id="account-retype-new-password"
                                          v-model="RetypePassword"
                                          :type="passwordFieldTypeRetype"
                                          name="retype-password"
                                          placeholder="Yeni şifrenizi tekrar yazınız" />
                            <b-input-group-append is-text>
                                <feather-icon :icon="passwordToggleIconRetype"
                                              class="cursor-pointer"
                                              @click="togglePasswordRetype" />
                            </b-input-group-append>
                        </b-input-group>
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
                              class="mt-1 mr-1">
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
            BFormValidFeedback
        },
        directives: {
            Ripple,
        },
        data() {
            return {
                passwordValueOld: '',
                newPasswordValue: '',
                RetypePassword: '',
                passwordFieldTypeOld: 'password',
                passwordFieldTypeNew: 'password',
                passwordFieldTypeRetype: 'password',
            }
        },
        computed: {
            oldPasswordValidation() {
            },
            preventLeadingSpace(e) {
                console.log(e)
                
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
        },
    }
</script>
