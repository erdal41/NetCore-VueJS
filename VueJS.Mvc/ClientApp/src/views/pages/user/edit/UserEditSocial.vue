﻿<template>
    <b-overlay :show="showOverlay"
               size="sm">
        <b-card>
            <b-form>
                <b-row>
                    <b-col cols="12">
                        <div class="d-flex align-items-center mb-2">
                            <feather-icon icon="LinkIcon"
                                          size="18" />
                            <h4 class="mb-0 ml-75">
                                Sosyal Medya
                            </h4>
                        </div>
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
                                              v-model="userUpdateDto.FacebookLink"
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
                                              v-model="userUpdateDto.TwitterLink"
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
                                              v-model="userUpdateDto.InstagramLink"
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
                                              v-model="userUpdateDto.LinkedInLink"
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
                                              v-model="userUpdateDto.YoutubeLink"
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
                                              v-model="userUpdateDto.GitHubLink"
                                              type="url"
                                              placeholder="GitHub profil linki" />
                            </b-input-group>
                        </b-form-group>
                    </b-col>

                    <b-col cols="12">
                        <hr class="my-2">
                    </b-col>

                    <!-- buttons -->
                    <b-col cols="12">
                        <b-spinner v-if="updateButtonDisabled"
                                   variant="secondary"
                                   class="align-middle mr-1" />
                        <b-button :disabled="updateButtonDisabled"
                                  :variant="updateButtonVariant" v-ripple.400="'rgba(255, 255, 255, 0.15)'"
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
                    <!--/ buttons -->
                </b-row>
            </b-form>
        </b-card>
    </b-overlay>
</template>

<script>
    import ToastificationContent from '@core/components/toastification/ToastificationContent.vue';
    import { BOverlay, BCard, BRow, BCol, BForm, BFormGroup, BInputGroup, BInputGroupPrepend, BFormInput, BSpinner, BButton } from 'bootstrap-vue'

    import Ripple from 'vue-ripple-directive'
    import axios from 'axios';

    export default {
        components: {
            ToastificationContent,
            BOverlay,
            BCard,
            BRow,
            BCol,
            BForm,
            BFormGroup,
            BInputGroup,
            BInputGroupPrepend,
            BFormInput,
            BSpinner,
            BButton
        },
        directives: {
            Ripple,
        },
        props: {
            socialData: {
                type: Object,
                default: () => { },
            },
        },
        data() {
            return {
                showOverlay: false,
                localOptions: JSON.parse(JSON.stringify(this.socialData)),
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
                                    text: response.data.User.UserName + ' adlı kullanıcının sosyal medya bilgileri güncellendi.'
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
