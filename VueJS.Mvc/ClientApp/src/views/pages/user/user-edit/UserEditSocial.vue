<template>
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
                                          v-model="localOptions.FacebookLink"
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
                                          v-model="localOptions.TwitterLink"
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
                                          v-model="localOptions.InstagramLink"
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
                                          v-model="localOptions.LinkedInLink"
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
                                          v-model="localOptions.YoutubeLink"
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
                                          v-model="localOptions.GitHubLink"
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
                <!--/ buttons -->
            </b-row>
        </b-form>
    </b-card>
</template>

<script>
    import {
        BButton, BForm, BFormGroup, BFormInput, BInputGroup, BInputGroupPrepend, BRow, BCol, BCard, BCardText, BLink, BAvatar,
    } from 'bootstrap-vue'
    import Ripple from 'vue-ripple-directive'

    export default {
        components: {
            BButton,
            BForm,
            BFormGroup,
            BFormInput,
            BInputGroup,
            BInputGroupPrepend,
            BRow,
            BCol,
            BCard,
            BCardText,
            BLink,
            BAvatar,
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
                localOptions: JSON.parse(JSON.stringify(this.socialData)),
            }
        },
        methods: {
            resetForm() {
                this.localOptions = JSON.parse(JSON.stringify(this.socialData))
            },
        },
    }
</script>
