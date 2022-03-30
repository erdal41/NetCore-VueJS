<template>
    <b-overlay :show="showOverlay"
               size="sm">
        <b-card>
            <!-- form -->
            <b-form>
                <b-row>
                    <b-col cols="12">
                        <div class="d-flex align-items-center mb-2">
                            <feather-icon icon="InfoIcon"
                                          size="18" />
                            <h4 class="mb-0 ml-75">
                                Kişisel Bilgiler
                            </h4>
                        </div>
                    </b-col>
                    <!-- bio -->
                    <b-col cols="12">
                        <b-form-group label="Biyografi"
                                      label-for="bio-area">
                            <b-form-textarea id="bio-area"
                                             v-model="userUpdateDto.About"
                                             rows="4"
                                             placeholder="Biyografi bilgilerinizi buraya girebilirsiniz..." />
                        </b-form-group>
                    </b-col>
                    <!--/ bio -->
                    <!-- website -->
                    <b-col md="6">
                        <b-form-group label-for="website"
                                      label="Website">
                            <b-form-input id="website"
                                          v-model="userUpdateDto.WebsiteLink"
                                          placeholder="Website adresiniz" />
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
                                        v-model="userUpdateDto.PhoneNumber"
                                        class="form-control"
                                        :raw="false"
                                        :options="maskOptions.phone"
                                        placeholder="123 123 12 12" />
                            </b-input-group>
                        </b-form-group>
                    </b-col>
                    <!-- phone -->

                    <b-col cols="12">
                        <hr class="my-2">
                    </b-col>

                    <b-col cols="12">
                        <b-spinner v-if="updateButtonDisabled"
                                   variant="secondary"
                                   class="align-middle mr-1" />
                        <b-button :disabled="updateButtonDisabled"
                                  :variant="updateButtonVariant"
                                  v-ripple.400="'rgba(255, 255, 255, 0.15)'"
                                  class="mr-1"
                                  @click.prevent="updateData">
                            Güncelle
                        </b-button>
                        <b-button v-ripple.400="'rgba(186, 191, 199, 0.15)'"
                                  type="reset"
                                  class="ml-25"
                                  variant="outline-secondary"
                                  @click.prevent="resetForm">
                            Tüm Değişiklikleri İptal Et
                        </b-button>
                    </b-col>
                </b-row>
            </b-form>
        </b-card>
    </b-overlay>
</template>

<script>
    import ToastificationContent from '@core/components/toastification/ToastificationContent.vue';
    import Cleave from 'vue-cleave-component'
    import { BOverlay, BCard, BRow, BCol, BForm, BFormGroup, BInputGroup, BInputGroupPrepend, BFormInput, BFormTextarea, BSpinner, BButton } from 'bootstrap-vue'
    import Ripple from 'vue-ripple-directive'
    import axios from 'axios';

    // eslint-disable-next-line import/no-extraneous-dependencies
    import 'cleave.js/dist/addons/cleave-phone.tr'

    export default {
        components: {
            ToastificationContent,
            Cleave,
            BOverlay,
            BCard,
            BRow,
            BCol,
            BForm,
            BFormGroup,
            BInputGroup,
            BInputGroupPrepend,
            BFormInput,
            BFormTextarea,
            BSpinner,
            BButton,
        },
        directives: {
            Ripple,
        },
        props: {
            informationData: {
                type: Object,
                default: () => { },
            },
        },
        data() {
            return {
                showOverlay: false,
                localOptions: JSON.parse(JSON.stringify(this.informationData)),
                maskOptions: {
                    phone: {
                        phone: true,
                        phoneRegionCode: 'TR',
                    },
                },
                userUpdateDto: {
                    Id: this.$route.query.edit,
                    ProfileImageId: null,
                    FirstName: null,
                    LastName: null,
                    UserName: null,
                    Email: null,
                    About: null,
                    WebsiteLink: null,
                    PhoneNumber: null,
                    FacebookLink: null,
                    TwitterLink: null,
                    InstagramLink: null,
                    LinkedInLink: null,
                    YoutubeLink: null,
                    GitHubLink: null,
                },
                updateButtonDisabled: false,
                updateButtonVariant: 'primary',
            }
        },
        methods: {
            getData() {
                this.showOverlay = true;
                if (this.localOptions) {
                    this.userUpdateDto.ProfileImageId = this.localOptions.ProfileImageId;
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
                    this.showOverlay = false;
                }
            },
            updateData() {
                this.updateButtonDisabled = true;
                this.updateButtonVariant = 'outline-secondary';
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
                                    text: response.data.User.UserName + ' adlı kullanıcının kişisel bilgileri güncellendi.'
                                }
                            });
                            this.updateButtonDisabled = false;
                            this.updateButtonVariant = 'primary';
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
            },
            resetForm() {
                this.getData();
            },
        },
        mounted() {
            this.getData();
        },
    }
</script>

<style lang="scss">
    @import '@core/scss/vue/libs/vue-select.scss';
</style>
