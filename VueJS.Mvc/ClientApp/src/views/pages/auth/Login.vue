<template>
    <div class="auth-wrapper auth-v1 px-2">
        <div class="auth-inner py-2">

            <!-- Login v1 -->
            <b-card class="mb-0">
                <b-link class="brand-logo">
                    <b-img v-bind:src="require('@/assets/images/media/' + logo)"
                           :alt="logoAltText"></b-img>
                </b-link>

                <!-- form -->
                <validation-observer ref="loginForm"
                                     #default="{invalid}">
                    <b-form class="auth-login-form mt-2"
                            @submit.prevent="login2">

                        <!-- email -->
                        <b-form-group label-for="email"
                                      label="E-Posta Adresi">
                            <validation-provider #default="{ errors }"
                                                 name="Email"
                                                 rules="required|email">
                                <b-form-input id="email"
                                              v-model="userEmail"
                                              name="login-email"
                                              :state="errors.length > 0 ? false:null"
                                              placeholder="john@example.com"
                                              autofocus />
                                <small class="text-danger">{{ errors[0] }}</small>
                            </validation-provider>
                        </b-form-group>

                        <!-- password -->
                        <b-form-group>
                            <div class="d-flex justify-content-between">
                                <label for="password">Parola</label>
                                <b-link :to="{name:'auth-forgot-password-v1'}">
                                    <small>Parolanızı mı unuttunuz?</small>
                                </b-link>
                            </div>
                            <validation-provider #default="{ errors }"
                                                 name="Password"
                                                 rules="required">
                                <b-input-group class="input-group-merge"
                                               :class="errors.length > 0 ? 'is-invalid':null">
                                    <b-form-input id="password"
                                                  v-model="password"
                                                  :type="passwordFieldType"
                                                  class="form-control-merge"
                                                  :state="errors.length > 0 ? false:null"
                                                  name="login-password"
                                                  placeholder="Parola" />

                                    <b-input-group-append is-text>
                                        <feather-icon class="cursor-pointer"
                                                      :icon="passwordToggleIcon"
                                                      @click="togglePasswordVisibility" />
                                    </b-input-group-append>
                                </b-input-group>
                                <small class="text-danger">{{ errors[0] }}</small>
                            </validation-provider>
                        </b-form-group>

                        <!-- checkbox -->
                        <b-form-group>
                            <b-form-checkbox id="remember-me"
                                             v-model="status"
                                             name="checkbox-1">
                                Beni Hatırla
                            </b-form-checkbox>
                        </b-form-group>

                        <!-- submit button -->
                        <b-button variant="primary"
                                  type="submit"
                                  block
                                  :disabled="invalid">
                            Giriş Yap
                        </b-button>
                    </b-form>
                </validation-observer>
                <!-- /Login v1 -->
            </b-card>
        </div>
    </div>
</template>

<script>
    import { ValidationProvider, ValidationObserver } from 'vee-validate'
    import {
        BImg, BButton, BForm, BFormInput, BFormGroup, BCard, BLink, BCardTitle, BCardText, BInputGroup, BInputGroupAppend, BFormCheckbox,
    } from 'bootstrap-vue'
    import VuexyLogo from '@core/layouts/components/Logo.vue'
    import { required, email } from '@validations'
    import { togglePasswordVisibility } from '@core/mixins/ui/forms'
    import useJwt from '@/auth/jwt/useJwt'
    import { getHomeRouteForLoggedInUser } from '@/auth/utils'
    import ToastificationContent from '@core/components/toastification/ToastificationContent.vue'
    import axios from 'axios'

    export default {
        components: {
            // BSV
            BImg,
            BButton,
            BForm,
            BFormInput,
            BFormGroup,
            BCard,
            BCardTitle,
            BLink,
            VuexyLogo,
            BCardText,
            BInputGroup,
            BInputGroupAppend,
            BFormCheckbox,
            ValidationProvider,
            ValidationObserver,
        },
        mixins: [togglePasswordVisibility],
        data() {
            return {
                password: 'admin',
                userEmail: 'admin@demo.com',
                status: '',
                // validation rules
                required,
                email,
                logo: '',
                logoAltText: '',
                userLoginDto: {
                    Email: '',
                    Password: '',
                    RememberMe: false
                },
            }
        },
        computed: {
            passwordToggleIcon() {
                return this.passwordFieldType === 'password' ? 'EyeIcon' : 'EyeOffIcon'
            },
        },
        methods: {
            getData() {
                axios.get('/admin/auth/login')
                    .then((response) => {
                        console.log(response.data);
                        this.logo = response.data.GeneralSettingDto.GeneralSetting.Logo.FileName;
                        this.logoAltText = response.data.GeneralSettingDto.GeneralSetting.Logo.AltText;
                    }).catch((error) => {
                        console.log(error);
                        console.log(error.request);
                        this.$toast({
                            component: ToastificationContent,
                            props: {
                                variant: 'danger',
                                title: 'Hata Oluştu!',
                                icon: 'AlertOctagonIcon',
                                text: 'Makaleler listenirken hata oluştu. Lütfen tekrar deneyiniz.',
                            }
                        })
                    });
            },
            login2() {
                this.$refs.loginForm.validate().then(success => {
                    if (success) {
                        useJwt.login({
                            email: this.userEmail,
                            password: this.password,
                        }).then((response) => {
                            console.log("asdasd: " + response.data);
                            if (response.data.UserLoginDto != null) {
                                const { userData } = response.data
                                useJwt.setToken(response.data.accessToken)
                                useJwt.setRefreshToken(response.data.refreshToken)
                                localStorage.setItem('userData', JSON.stringify(userData))
                                this.$router.replace(getHomeRouteForLoggedInUser("admin"))
                                    .then(() => {
                                        this.$toast({
                                            component: ToastificationContent,
                                            position: 'top-right',
                                            props: {
                                                title: `Hoşgeldin ${response.data.UserLoginDto.FirstName || response.data.UserLoginDto.LastName}`,
                                                icon: 'CoffeeIcon',
                                                variant: 'success',
                                                text: `admin olarak başarıyla giriş yaptınız.`,
                                            },
                                        })
                                    })
                            }
                            else {
                                this.$toast({
                                    component: ToastificationContent,
                                    props: {
                                        variant: 'danger',
                                        title: 'Hata!',
                                        icon: 'AlertOctagonIcon',
                                        text: 'E-posta adresiniz veya parolanız yanlış olabilir. Lütfen kontrol ediniz.',
                                    }
                                })
                            }
                        }).catch(error => {
                            this.$refs.loginForm.setErrors(error.response.data.error)
                            console.log(error.response.data.error)
                        })
                    }
                })
            },
            login1() {
                this.$refs.loginForm.validate().then(success => {
                    if (success) {
                        useJwt.login({
                            email: this.userEmail,
                            password: this.password,
                        })
                            .then(response => {
                                console.log(response.data);
                                const { userData } = response.data
                                useJwt.setToken(response.data.accessToken)
                                useJwt.setRefreshToken(response.data.refreshToken)
                                localStorage.setItem('userData', JSON.stringify(userData))
                                this.$ability.update(userData.ability)

                                // ? This is just for demo purpose as well.
                                // ? Because we are showing eCommerce app's cart items count in navbar
                                this.$store.commit('app-ecommerce/UPDATE_CART_ITEMS_COUNT', userData.extras.eCommerceCartItemsCount)

                                // ? This is just for demo purpose. Don't think CASL is role based in this case, we used role in if condition just for ease
                                this.$router.replace(getHomeRouteForLoggedInUser(userData.role))
                                    .then(() => {
                                        this.$toast({
                                            component: ToastificationContent,
                                            position: 'top-right',
                                            props: {
                                                title: `Hoşgeldin ${userData.fullName || userData.username}`,
                                                icon: 'CoffeeIcon',
                                                variant: 'success',
                                                text: `${userData.role} olarak başarıyla giriş yaptınız.`,
                                            },
                                        })
                                    })
                            })
                            .catch(error => {
                                this.$refs.loginForm.setErrors(error.response.data.error)
                            })
                    }
                })
            },
        },
        mounted() {
            this.getData();
        }
    }
</script>

<style lang="scss">
    @import '@core/scss/vue/pages/page-auth.scss';
</style>
