<template>
    <b-row class="content-header">
        <modal-media @changeImage="imageChange"></modal-media>
        <b-col class="content-header-left mb-2"
               cols="12"
               md="8">
            <h2 class="content-header-title float-left pr-1 mb-0">
                SEO Ayarları
            </h2>
            <div class="breadcrumb-wrapper">
                <b-breadcrumb>
                    <b-breadcrumb-item to="/">
                        <feather-icon icon="HomeIcon"
                                      size="16"
                                      class="align-text-top" />
                    </b-breadcrumb-item>
                    <b-breadcrumb-item active>SEO Ayarlarını Düzenle</b-breadcrumb-item>
                </b-breadcrumb>
            </div>
        </b-col>
        <b-col class="content-header-right text-md-right d-md-block d-none mb-1"
               md="4"
               cols="12">
            <b-button v-if="$can('create', 'User')"
                      variant="primary"
                      type="button"
                      :to="{name: 'pages-user-add'}">
                Yeni Ekle
            </b-button>
        </b-col>
        <b-col cols="12">
            <b-tabs id="tab-seo-settings"
                    vertical
                    content-class="col-12 col-md-9 mt-1 mt-md-0"
                    pills
                    nav-wrapper-class="col-md-3 col-12"
                    nav-class="nav-left">
                <b-tab active>
                    <template #title>
                        <feather-icon icon="CpuIcon"
                                      size="18"
                                      class="mr-50" />
                        <span class="font-weight-bold">Genel</span>
                    </template>
                    <b-tabs>
                        <b-tab title="Web Yöneticisi Araçları">
                            <b-card>
                                <b-form-group label="Google Doğrulama Kodu"
                                              label-for="googleVerificationCode">
                                    <b-form-input id="googleVerificationCode"
                                                  placeholder="Google doğrulama kodunu giriniz." />
                                </b-form-group>
                                <b-form-group label="Yandex Doğrulama Kodu"
                                              label-for="yandexVerificationCode">
                                    <b-form-input id="yandexVerificationCode"
                                                  placeholder="Yandex doğrulama kodunu giriniz." />
                                </b-form-group>
                                <b-form-group label="Bing Doğrulama Kodu"
                                              label-for="bingVerificationCode">
                                    <b-form-input id="bingVerificationCode"
                                                  placeholder="Bing doğrulama kodunu giriniz." />
                                </b-form-group>
                                <b-form-group label="Baidu Doğrulama Kodu"
                                              label-for="baiduVerificationCode">
                                    <b-form-input id="baiduVerificationCode"
                                                  placeholder="Bauidu doğrulama kodunu giriniz." />
                                </b-form-group>
                            </b-card>
                        </b-tab>
                        <b-tab title="Özellikler">
                            <b-card>
                                <b-form-group>
                                    <b-form-checkbox v-model="isActiveSitemap"
                                                     name="check-button"
                                                     switch
                                                     inline>
                                        XML site haritaları aktif olsun mu?
                                    </b-form-checkbox>
                                    <a href="#">Görüntüle</a>
                                </b-form-group>
                                <b-form-group>
                                    <b-form-checkbox v-model="isActiveRobotsTxt"
                                                     name="check-button"
                                                     switch
                                                     inline>
                                        Robots.txt dosyası aktif olsun mu?
                                    </b-form-checkbox>
                                    <a href="#">Görüntüle</a>
                                </b-form-group>
                            </b-card>
                        </b-tab>
                    </b-tabs>
                </b-tab>
                <b-tab>
                    <template #title>
                        <feather-icon icon="AirplayIcon"
                                      size="18"
                                      class="mr-50" />
                        <span class="font-weight-bold">Arama Görünürlüğü</span>
                    </template>
                    <b-tabs>
                        <b-tab title="Genel">

                        </b-tab>
                        <b-tab title="Sayfa - Makale">
                            <b-card-actions id="card-page"
                                            title="Sayfalar"
                                            action-collapse
                                            collapsed>
                                <b-form-group>
                                    <b-form-checkbox v-model="isActivePageSearchEngineIndex"
                                                     name="check-button"
                                                     switch
                                                     inline>
                                        Sayfalar arama sonuçlarında gösterilsin mi?
                                    </b-form-checkbox>
                                </b-form-group>
                                <b-form-group>
                                    <b-form-checkbox v-model="isActivePageSeoSetting"
                                                     name="check-button"
                                                     switch
                                                     inline>
                                        Sayfalar için SEO ayarları gösterilsin mi?
                                    </b-form-checkbox>
                                </b-form-group>
                                <b-form-group label="Varsayılan SEO Başlığı"
                                              label-for="pageDefaultSeoTitle">
                                    <b-form-input id="pageDefaultSeoTitle"
                                                  placeholder="SEO başlığını giriniz." />
                                </b-form-group>
                                <b-form-group label="Varsayılan Meta Açıklama"
                                              label-for="pageDefaultSeoDescription">
                                    <b-form-textarea id="pageDefaultSeoDescription"
                                                     placeholder="Meta açıklamanızı giriniz."
                                                     rows="2" />
                                </b-form-group>
                                <b-form-group label="Sayfa Türü"
                                              label-for="pageSchemaPageType">
                                    <v-select id="pageSchemaPageType"
                                              v-model="pageSchemaPageType"
                                              :options="pageSchemaPageTypes"
                                              label="Name"
                                              :reduce="(option) => option.Id"
                                              :clearable="false" />
                                </b-form-group>
                                <b-form-group label="Makale Türü"
                                              label-for="pageSchemaArticleType">
                                    <v-select id="pageSchemaArticleType"
                                              v-model="pageSchemaArticleType"
                                              :options="pageSchemaArticleTypes"
                                              label="Name"
                                              :reduce="(option) => option.Id"
                                              :clearable="false" />
                                </b-form-group>

                                <h5 class="mt-3 mb-2">Sayfalar İçin Varsayılan Sosyal Medya Ayarları</h5>
                                <div class="image-preview">
                                    <div class="image-thumbnail select-pagesocial-image"
                                         v-b-modal.modal-media
                                         @click="selectImage">
                                        <b-img rounded
                                               v-bind:src="pageSocialImage.fileName == null ? noImage : require('@/assets/images/media/' + pageSocialImage.fileName)"
                                               :alt="pageSocialImage.altText"
                                               class="select-pagesocial-image" />
                                        <b-button v-ripple.400="'rgba(186, 191, 199, 0.15)'"
                                                  variant="relief-primary"
                                                  size="sm"
                                                  class="btn-icon rounded-circle select-image select-pagesocial-image">
                                            <feather-icon icon="Edit2Icon"
                                                          class="select-pagesocial-image"
                                                          size="11" />
                                        </b-button>
                                    </div>
                                    <b-button v-ripple.400="'rgba(186, 191, 199, 0.15)'"
                                              variant="relief-secondary"
                                              size="sm"
                                              class="btn-icon rounded-circle remove-image remove-pagesocial-image"
                                              @click="removeImage">
                                        <feather-icon icon="XIcon"
                                                      class="remove-pagesocial-image"
                                                      size="11" />
                                    </b-button>
                                    <b-form-input type="text"
                                                  hidden
                                                  v-model="pageSocialImage.id"></b-form-input>
                                </div>
                                <b-form-group label="Sosyal Medya Paylaşım Başlığı"
                                              label-for="pageSocialTitle"
                                              class="mt-1">
                                    <b-form-input id="pageSocialTitle"
                                                  v-model="pageSocialTitle"
                                                  type="text"
                                                  placeholder="Sosyal medya başlığını giriniz." />
                                </b-form-group>
                                <b-form-group label="Sosyal Medya Paylaşım Açıklaması"
                                              label-for="pageSocialDescription">
                                    <b-form-textarea id="pageSocialDescription"
                                                     v-model="pageSocialDescription"
                                                     placeholder="Sosyal medya açıklamasını giriniz."
                                                     rows="2" />
                                </b-form-group>
                            </b-card-actions>
                            <b-card-actions id="card-article"
                                            title="Makaleler"
                                            action-collapse
                                            collapsed>
                                <b-form-group>
                                    <b-form-checkbox v-model="isActiveArticleSearchEngineIndex"
                                                     name="check-button"
                                                     switch
                                                     inline>
                                        Makaleler arama sonuçlarında gösterilsin mi?
                                    </b-form-checkbox>
                                </b-form-group>
                                <b-form-group>
                                    <b-form-checkbox v-model="isActiveArticleSeoSetting"
                                                     name="check-button"
                                                     switch
                                                     inline>
                                        Makaleler için SEO ayarları gösterilsin mi?
                                    </b-form-checkbox>
                                </b-form-group>
                                <b-form-group label="Varsayılan SEO Başlığı"
                                              label-for="articleDefaultSeoTitle">
                                    <b-form-input id="articleDefaultSeoTitle"
                                                  placeholder="SEO başlığını giriniz." />
                                </b-form-group>
                                <b-form-group label="Varsayılan Meta Açıklama"
                                              label-for="articleDefaultSeoDescription">
                                    <b-form-textarea id="articleDefaultSeoDescription"
                                                     placeholder="Meta açıklamanızı giriniz."
                                                     rows="2" />
                                </b-form-group>
                                <b-form-group label="Sayfa Türü"
                                              label-for="articleSchemaPageType">
                                    <v-select id="articleSchemaPageType"
                                              v-model="articleSchemaPageType"
                                              :options="articleSchemaPageTypes"
                                              label="Name"
                                              :reduce="(option) => option.Id"
                                              :clearable="false" />
                                </b-form-group>
                                <b-form-group label="Makale Türü"
                                              label-for="articleSchemaArticleType">
                                    <v-select id="articleSchemaArticleType"
                                              v-model="articleSchemaArticleType"
                                              :options="articleSchemaArticleTypes"
                                              label="Name"
                                              :reduce="(option) => option.Id"
                                              :clearable="false" />
                                </b-form-group>

                                <h5 class="mt-3 mb-2">Makaleler İçin Varsayılan Sosyal Medya Ayarları</h5>
                                <div class="image-preview">
                                    <div class="image-thumbnail select-articlesocial-image"
                                         v-b-modal.modal-media
                                         @click="selectImage">
                                        <b-img rounded
                                               v-bind:src="articleSocialImage.fileName == null ? noImage : require('@/assets/images/media/' + articleSocialImage.fileName)"
                                               :alt="articleSocialImage.altText"
                                               class="select-articlesocial-image" />
                                        <b-button v-ripple.400="'rgba(186, 191, 199, 0.15)'"
                                                  variant="relief-primary"
                                                  size="sm"
                                                  class="btn-icon rounded-circle select-image select-articlesocial-image">
                                            <feather-icon icon="Edit2Icon"
                                                          class="select-articlesocial-image"
                                                          size="11" />
                                        </b-button>
                                    </div>
                                    <b-button v-ripple.400="'rgba(186, 191, 199, 0.15)'"
                                              variant="relief-secondary"
                                              size="sm"
                                              class="btn-icon rounded-circle remove-image remove-articlesocial-image"
                                              @click="removeImage">
                                        <feather-icon icon="XIcon"
                                                      class="remove-articlesocial-image"
                                                      size="11" />
                                    </b-button>
                                    <b-form-input type="text"
                                                  hidden
                                                  v-model="articleSocialImage.id"></b-form-input>
                                </div>
                                <b-form-group label="Sosyal Medya Paylaşım Başlığı"
                                              label-for="articleSocialTitle"
                                              class="mt-1">
                                    <b-form-input id="articleSocialTitle"
                                                  v-model="articleSocialTitle"
                                                  type="text"
                                                  placeholder="Sosyal medya başlığını giriniz." />
                                </b-form-group>
                                <b-form-group label="Sosyal Medya Paylaşım Açıklaması"
                                              label-for="articleSocialDescription">
                                    <b-form-textarea id="articleSocialDescription"
                                                     v-model="articleSocialDescription"
                                                     placeholder="Sosyal medya açıklamasını giriniz."
                                                     rows="2" />
                                </b-form-group>
                            </b-card-actions>
                        </b-tab>
                        <b-tab title="Kategori - Etiket">

                        </b-tab>
                    </b-tabs>
                </b-tab>
                <b-tab>
                    <template #title>
                        <feather-icon icon="Share2Icon"
                                      size="18"
                                      class="mr-50" />
                        <span class="font-weight-bold">Sosyal Ağlar</span>
                    </template>
                    <b-tabs>
                        <b-tab title="Hesaplar">

                        </b-tab>
                        <b-tab title="Sosyal Medya">

                        </b-tab>
                        <b-tab title="Twitter">

                        </b-tab>
                        <b-tab title="Pinterest">

                        </b-tab>
                    </b-tabs>
                </b-tab>
            </b-tabs>
        </b-col>
    </b-row>
</template>

<script>
    import ModalMedia from '../media/ModalMedia.vue';
    import BCardActions from '@core/components/b-card-actions/BCardActions.vue';
    import vSelect from 'vue-select';
    import { BRow, BCol, BBreadcrumb, BBreadcrumbItem, BButton, BTabs, BTab, BCard, BSpinner, BImg, BFormGroup, BFormInput, BFormCheckbox, BFormTextarea } from 'bootstrap-vue';
    import axios from 'axios';
    import ToastificationContent from '@core/components/toastification/ToastificationContent.vue';
    import Ripple from 'vue-ripple-directive'

    export default {
        components: {
            ModalMedia,
            BCardActions,
            vSelect,
            BRow,
            BCol,
            BBreadcrumb,
            BBreadcrumbItem,
            BButton,
            BTabs,
            BTab,
            BCard,
            BSpinner,
            BImg,
            BFormGroup,
            BFormInput,
            BFormCheckbox,
            BFormTextarea
        },
        directives: {
            Ripple,
        },
        data() {
            return {
                noImage: require('@/assets/images/default/default-post-image.jpg'),
                googleVerificationCode: '',
                yandexVerificationCode: '',
                bingVerificationCode: '',
                baiduVerificationCode: '',
                isActiveSitemap: false,
                isActiveRobotsTxt: false,
                isPageSocialImageChoose: false,
                isArticleSocialImageChoose: false,
                isCategorySocialImageChoose: false,
                isTagSocialImageChoose: false,
                pageSocialImage: {
                    id: null,
                    fileName: null,
                    altText: null,
                },
                articleSocialImage: {
                    id: null,
                    fileName: null,
                    altText: null,
                },
                categorySocialImage: {
                    id: null,
                    fileName: null,
                    altText: null,
                },
                tagSocialImage: {
                    id: null,
                    fileName: null,
                    altText: null,
                },
                isActivePageSearchEngineIndex: false,
                isActivePageSeoSetting: false,
                pageDefaultSeoTitle: '',
                pageDefaultSeoDescription: '',
                pageSchemaPageType: '',
                pageSchemaPageTypes: [],
                pageSchemaArticleType: '',
                pageSchemaArticleTypes: [],
                pageSocialTitle: '',
                pageSocialDescription: '',
                isActiveArticleSearchEngineIndex: false,
                isActiveArticleSeoSetting: false,
                articleDefaultSeoTitle: '',
                articleDefaultSeoDescription: '',
                articleSchemaPageType: '',
                articleSchemaPageTypes: [],
                articleSchemaArticleType: '',
                articleSchemaArticleTypes: [],
                articleSocialTitle: '',
                articleSocialDescription: '',

            }
        },
        methods: {
            imageChange(id, name, altText) {
                if (this.isPageSocialImageChoose == true) {
                    this.pageSocialImage.id = id;
                    this.pageSocialImage.fileName = name;
                    this.pageSocialImage.altText = altText;
                } else if (this.isArticleSocialImageChoose == true) {
                    this.articleSocialImage.id = id;
                    this.articleSocialImage.fileName = name;
                    this.articleSocialImage.altText = altText;
                } else if (this.isCategorySocialImageChoose == true) {
                    this.categorySocialImage.id = id;
                    this.categorySocialImage.fileName = name;
                    this.categorySocialImage.altText = altText;
                } else if (this.isTagSocialImageChoose == true) {
                    this.tagSocialImage.id = id;
                    this.tagSocialImage.fileName = name;
                    this.tagSocialImage.altText = altText;
                }

                this.isPageSocialImageChoose = false;
                this.isArticleSocialImageChoose = false;
                this.isCategorySocialImageChoose = false;
                this.isTagSocialImageChoose = false;
            },
            selectImage(e) {
                if (e.target._prevClass.includes('select-pagesocial-image')) {
                    this.isPageSocialImageChoose = true;
                } else if (e.target._prevClass.includes('select-articlesocial-image')) {
                    this.isArticleSocialImageChoose = true;
                } else if (e.target._prevClass.includes('select-categorysocial-image')) {
                    this.isCategorySocialImageChoose = true;
                } else if (e.target._prevClass.includes('select-tagsocial-image')) {
                    this.isTagSocialImageChoose = true;
                }
            },
            removeImage: function (e) {
                if (e.target._prevClass.includes('remove-pagesocial-image')) {
                    this.pageSocialImage.id = null;
                    this.pageSocialImage.fileName = null;
                    this.pageSocialImage.altText = null;
                } else if (e.target._prevClass.includes('remove-articlesocial-image')) {
                    this.articleSocialImage.id = null;
                    this.articleSocialImage.fileName = null;
                    this.articleSocialImage.altText = null;
                } else if (e.target._prevClass.includes('remove-categorysocial-image')) {
                    this.categorySocialImage.id = null;
                    this.categorySocialImage.fileName = null;
                    this.categorySocialImage.altText = null;
                }
                else if (e.target._prevClass.includes('remove-tagsocial-image')) {
                    this.tagSocialImage.id = null;
                    this.tagSocialImage.fileName = null;
                    this.tagSocialImage.altText = null;
                }
            },
        },
        mounted() {
        }
    }
</script>

<style lang="scss">
    @import '@core/scss/vue/libs/vue-select.scss';

    #tab-general-settings .card .card-header {
        padding: 8px 8px 8px 20px;
        border-bottom: 1px solid #ebe9f1;
        margin-bottom: 20px;
    }

    .image-preview {
        width: 300px;
        height: 200px;
        position: relative;
    }

    .image-thumbnail {
        width: 100%;
        height: 200px;
        -webkit-box-shadow: 0px 0px 3px 0px rgba(196,196,196,1);
        -moz-box-shadow: 0px 0px 3px 0px rgba(196,196,196,1);
        box-shadow: 0px 0px 3px 0px rgba(196,196,196,1);
        position: relative;
        border-radius: 5px;
    }

    .image-thumbnail img {
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

    .image-preview .select-image {
        position: absolute !important;
        top: -12px;
        right: -12px;
        padding: 5px !important;
    }

    .image-preview .remove-image {
        position: absolute !important;
        bottom: -12px;
        right: -12px;
        padding: 5px !important;
    }
</style>