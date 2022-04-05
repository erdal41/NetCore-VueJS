<template>
    <b-row class="content-header">
        <b-col class="content-header-left mb-2"
               cols="12"
               md="6">
            <b-row class="breadcrumbs-top">
                <b-col cols="12">
                    <h2 class="content-header-title float-left pr-1 mb-0">
                        Sayfa Ekle
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
        <b-col class="content-header-right text-md-right d-md-block d-none mb-1"
               md="6"
               cols="12">
            <b-spinner v-if="buttonDisabled"
                       variant="secondary"
                       class="align-middle mr-1" />
            <b-button id="draft"
                      :disabled="buttonDisabled"
                      variant="outline-primary"
                      class="mr-1"
                      size="sm"
                      type="submit"
                      @click.prevent="validationForm">
                Taslak Olarak Kaydet
            </b-button>
            <b-button id="save"
                      :disabled="buttonDisabled"
                      :variant="saveButtonVariant"
                      type="submit"
                      @click.prevent="validationForm">
                Yayınla
            </b-button>
        </b-col>
        <modal-media @changeImage="imageChange"></modal-media>
        <b-col md="12"
               lg="8">
            <b-card>
                <validation-observer ref="pageAddForm">
                    <b-form>
                        <b-row>
                            <b-col cols="12">
                                <b-form-group label-for="title">
                                    <validation-provider #default="{ errors }"
                                                         name="title"
                                                         vid="title"
                                                         rules="required">
                                        <b-form-input id="title"
                                                      v-model="postAddDto.Title"
                                                      :state="errors.length > 0 ? false:null"
                                                      type="text"
                                                      placeholder="Başlık"
                                                      @blur="changePostName" />
                                        <small class="text-danger">{{ errors[0] }}</small>
                                    </validation-provider>
                                </b-form-group>
                                <b-form-group v-if="isShowPostName">
                                    <span class="small">Gönderi linki: </span><a class="small" :href="domainName + parentPostName + '/' + postAddDto.PostName">{{ domainName }}{{ parentPostName }}/<span v-show="isSlugEditActive == false">{{  postAddDto.PostName }}</span></a>
                                    <b-button v-if="isSlugEditActive == false"
                                              v-b-tooltip.hover
                                              title="Gönderinin linkini değiştirmenizi sağlar."
                                              v-ripple.400="'rgba(113, 102, 240, 0.15)'"
                                              variant="outline-primary"
                                              size="sm"
                                              class="ml-1"
                                              @click="isSlugEditActive = !isSlugEditActive">
                                        Düzenle
                                    </b-button>
                                    <div v-show="isSlugEditActive == true"
                                         class="card-border p-1">
                                        <b-form-input type="text"
                                                      v-model="postAddDto.PostName"
                                                      placeholder="Gönderi Linki"
                                                      size="sm">
                                        </b-form-input>
                                        <b-button variant="primary"
                                                  class="mt-1"
                                                  size="sm"
                                                  @click="postNameEdit">
                                            Tamam
                                        </b-button>
                                        <b-button variant="flat-secondary"
                                                  size="sm"
                                                  class="ml-1 mt-1"
                                                  @click="postNameEditCancel">
                                            İptal
                                        </b-button>
                                    </div>
                                </b-form-group>
                                <quill-editor v-model="postAddDto.Content"
                                              :options="editorOption" />
                            </b-col>
                        </b-row>
                    </b-form>
                </validation-observer>
            </b-card>
            <b-card-actions v-show="$can('create','Seo')"
                            title="SEO Ayarları"
                            action-collapse
                            collapsed>
                <b-tabs>
                    <b-tab title="Genel"
                           active>
                        <b-form>
                            <b-row>
                                <b-col cols="12">
                                    <b-form-group label-for="SeoTitle"
                                                  description="Arama motoru optimizasyonu için geçerli olan SEO Başlığının uzunluğu 60 karakter olarak önerilir.">
                                        <b-form-input id="SeoTitle"
                                                      v-model="pageSeoSettingAddDto.SeoTitle"
                                                      type="text"
                                                      placeholder="Seo Başlığı" />
                                    </b-form-group>
                                    <b-form-group label-for="keyword"
                                                  description="Bazı arama motorları için hala geçerli olan kelimeler, içerikle ilgili olacak şekilde maksimum 5 adet girilmesi önerilir.">
                                        <b-form-tags v-model="keywords"
                                                     input-id="tags-basic"
                                                     id="keyword"
                                                     class="mb-2"
                                                     placeholder="Anahtar kelime giriniz." />
                                    </b-form-group>
                                    <b-form-group label-for="SeoDescription"
                                                  description="Arama motoru optimizayonu için geçerli olan SEO açıklamasının uzunluğu 50-160 karakter arasında girilmesi önerilir.">
                                        <b-form-textarea id="SeoDescription"
                                                         v-model="pageSeoSettingAddDto.SeoDescription"
                                                         placeholder="Meta Açıklama"
                                                         rows="3" />
                                    </b-form-group>
                                    <app-collapse>
                                        <app-collapse-item title="Gelişmiş">
                                            <b-form-group label-for="CanonicalUrl"
                                                          description="Geçerli sayfa ile benzer içeriğe sahip olan sayfanın linkini giriniz.">
                                                <b-form-input id="CanonicalUrl"
                                                              v-model="pageSeoSettingAddDto.CanonicalUrl"
                                                              type="text"
                                                              placeholder="Benzer Link" />
                                            </b-form-group>
                                            <b-form-group>
                                                <b-form-checkbox v-model="pageSeoSettingAddDto.IsRobotsNoIndex"
                                                                 name="check-button"
                                                                 switch
                                                                 inline>
                                                    Arama motorlarının bu terimi arama sonuçlarında göstermesini istiyor musunuz?
                                                </b-form-checkbox>
                                            </b-form-group>
                                            <b-form-group>
                                                <b-form-checkbox v-model="pageSeoSettingAddDto.IsRobotsNoFollow"
                                                                 name="check-button"
                                                                 switch
                                                                 inline>
                                                    Arama motorları bu sayfa üzerindeki bağlantıları(linkleri) izlemeli mi?
                                                </b-form-checkbox>
                                            </b-form-group>
                                            <b-form-group>
                                                <b-form-checkbox v-model="pageSeoSettingAddDto.IsRobotsNoArchive"
                                                                 name="check-button"
                                                                 switch
                                                                 inline>
                                                    Arama motorları bu sayfanın önbelleğini almalı mı?
                                                </b-form-checkbox>
                                            </b-form-group>
                                            <b-form-group>
                                                <b-form-checkbox v-model="pageSeoSettingAddDto.IsRobotsNoImageIndex"
                                                                 name="check-button"
                                                                 switch
                                                                 inline>
                                                    Arama motorları bu sayfa üzerindeki resimleri indexlemeli mi?
                                                </b-form-checkbox>
                                            </b-form-group>
                                            <b-form-group>
                                                <b-form-checkbox v-model="pageSeoSettingAddDto.IsRobotsNoSnippet"
                                                                 name="check-button"
                                                                 switch
                                                                 inline>
                                                    Arama motorları bu sayfa için özel snippetlere izin vermeli mi?
                                                </b-form-checkbox>
                                            </b-form-group>
                                        </app-collapse-item>
                                    </app-collapse>
                                </b-col>
                            </b-row>
                        </b-form>
                    </b-tab>
                    <b-tab title="Şema">
                        <p>
                            <strong>Sayfalarınızı schema.org kullanarak otomatik olarak tanımlar</strong>
                            <br />
                            Bu, arama motorlarının web sitenizi ve içeriğinizi anlamasına yardımcı olur. Bu sayfa için bazı ayarlarınızı aşağıda değiştirebilirsiniz.
                        </p>
                        <b-form-group label="Sayfa Türü">
                            <v-select id="schemaPageType"
                                      v-model="pageSeoSettingAddDto.SchemaPageType"
                                      :options="schemaPageTypes"
                                      label="Name"
                                      :reduce="(option) => option.Id"
                                      placeholder="— Sayfa Türü —"
                                      :disabled="isSchemaPageSelectLoading"
                                      :loading="isSchemaPageSelectLoading">
                                <template #spinner="{ loading }">
                                    <div v-if="isSchemaPageSelectLoading"
                                         style="border-left-color: rgba(88, 151, 251, 0.71)"
                                         class="vs__spinner">
                                    </div>
                                </template>
                                <template #no-options="{ search, searching, loading }">
                                    {{ nullSchemaMessage }}
                                </template>
                            </v-select>
                        </b-form-group>
                        <b-form-group label="Makale Türü">
                            <v-select id="schemaArticleType"
                                      v-model="pageSeoSettingAddDto.SchemaArticleType"
                                      :options="schemaArticleTypes"
                                      label="Name"
                                      :reduce="(option) => option.Id"
                                      placeholder="— Makale Türü —"
                                      :disabled="isSchemaArticleSelectLoading"
                                      :loading="isSchemaArticleSelectLoading">
                                <template #spinner="{ loading }">
                                    <div v-if="isSchemaArticleSelectLoading"
                                         style="border-left-color: rgba(88, 151, 251, 0.71)"
                                         class="vs__spinner">
                                    </div>
                                </template>
                                <template #no-options="{ search, searching, loading }">
                                    {{ nullSchemaMessage }}
                                </template>
                            </v-select>
                        </b-form-group>
                    </b-tab>
                    <b-tab title="Sosyal Medya">
                        <app-collapse>
                            <app-collapse-item title="Sosyal Medya">
                                <b-row class="kb-search-content-info match-height">
                                    <b-col lg="4"
                                           md="4"
                                           sm="6">
                                        <div class="image-preview">
                                            <div class="image-thumbnail select-opengraph-image"
                                                 v-b-modal.modal-media
                                                 @click="selectImage">
                                                <b-img rounded
                                                       v-bind:src="openGraphImage.fileName == null ? noImage : require('@/assets/images/media/' + openGraphImage.fileName)"
                                                       :alt="openGraphImage.altText"
                                                       class="select-opengraph-image" />
                                                <b-button v-ripple.400="'rgba(186, 191, 199, 0.15)'"
                                                          variant="relief-primary"
                                                          size="sm"
                                                          class="btn-icon rounded-circle select-image select-opengraph-image">
                                                    <feather-icon icon="Edit2Icon"
                                                                  class="select-opengraph-image"
                                                                  size="11" />
                                                </b-button>
                                            </div>
                                            <b-button v-ripple.400="'rgba(186, 191, 199, 0.15)'"
                                                      variant="relief-secondary"
                                                      size="sm"
                                                      class="btn-icon rounded-circle remove-image remove-opengraph-image"
                                                      @click="removeImage">
                                                <feather-icon icon="XIcon"
                                                              class="remove-opengraph-image"
                                                              size="11" />
                                            </b-button>
                                            <b-form-input type="text"
                                                          hidden
                                                          v-model="openGraphImage.id"></b-form-input>
                                        </div>
                                    </b-col>
                                </b-row>
                                <b-form-group class="mt-1">
                                    <b-form-input id="OpenGraphTitle"
                                                  v-model="pageSeoSettingAddDto.OpenGraphTitle"
                                                  type="text"
                                                  placeholder="Sosyal Medya Başlığı" />
                                </b-form-group>
                                <b-form-group>
                                    <b-form-textarea id="OpenGraphDescription"
                                                     v-model="pageSeoSettingAddDto.OpenGraphDescription"
                                                     placeholder="Sosyal Medya Açıklaması"
                                                     rows="3" />
                                </b-form-group>
                            </app-collapse-item>
                            <app-collapse-item title="Twitter">
                                <b-row class="kb-search-content-info match-height">
                                    <b-col lg="4"
                                           md="4"
                                           sm="6">
                                        <div class="image-preview">
                                            <div class="image-thumbnail select-twitter-image"
                                                 v-b-modal.modal-media
                                                 @click="selectImage">
                                                <b-img rounded
                                                       v-bind:src="twitterImage.fileName == null ? noImage : require('@/assets/images/media/' + twitterImage.fileName)"
                                                       :alt="twitterImage.altText"
                                                       class="select-twitter-image" />
                                                <b-button v-ripple.400="'rgba(186, 191, 199, 0.15)'"
                                                          variant="relief-primary"
                                                          size="sm"
                                                          class="btn-icon rounded-circle select-image select-twitter-image">
                                                    <feather-icon icon="Edit2Icon"
                                                                  class="select-twitter-image"
                                                                  size="11" />
                                                </b-button>
                                            </div>
                                            <b-button v-ripple.400="'rgba(186, 191, 199, 0.15)'"
                                                      variant="relief-secondary"
                                                      size="sm"
                                                      class="btn-icon rounded-circle remove-image remove-twitter-image"
                                                      @click="removeImage">
                                                <feather-icon icon="XIcon"
                                                              class="remove-twitter-image"
                                                              size="11" />
                                            </b-button>
                                            <b-form-input type="text"
                                                          hidden
                                                          v-model="twitterImage.id"></b-form-input>
                                        </div>
                                    </b-col>
                                </b-row>
                                <b-form-group class="mt-1">
                                    <b-form-input id="TwitterTitle"
                                                  v-model="pageSeoSettingAddDto.TwitterTitle"
                                                  type="text"
                                                  placeholder="Twitter Başlığı" />
                                </b-form-group>
                                <b-form-group>
                                    <b-form-textarea id="TwitterDescription"
                                                     v-model="pageSeoSettingAddDto.TwitterDescription"
                                                     placeholder="Twitter Açıklaması"
                                                     rows="3" />
                                </b-form-group>
                            </app-collapse-item>
                        </app-collapse>
                    </b-tab>
                </b-tabs>
            </b-card-actions>
        </b-col>
        <b-col md="12"
               lg="4">
            <b-card-actions title="Ebeveyn Sayfa"
                            action-collapse>
                <b-form-group>
                    <v-select v-model="postAddDto.ParentId"
                              :options="topPosts"
                              label="Title"
                              :reduce="(option) => option.Id"
                              placeholder="— Ebeveyn Sayfa —"
                              :disabled="isTopPageSelectLoading"
                              :loading="isTopPageSelectLoading">
                        <template #spinner="{ loading }">
                            <div v-if="isTopPageSelectLoading"
                                 style="border-left-color: rgba(88, 151, 251, 0.71)"
                                 class="vs__spinner">
                            </div>
                        </template>
                        <template #no-options="{ search, searching, loading }">
                            {{ nullPageMessage }}
                        </template>
                    </v-select>
                </b-form-group>
            </b-card-actions>
            <b-card-actions title="Resim"
                            action-collapse>
                <b-form-group>
                    <b-form-checkbox v-model="postAddDto.IsShowFeaturedImage"
                                     name="check-button"
                                     switch
                                     inline>
                        Görsel gönderide gösterilsin mi?
                    </b-form-checkbox>
                </b-form-group>
                <div class="image-preview mt-1">
                    <div class="image-thumbnail select-featured-image"
                         v-b-modal.modal-media
                         @click="selectImage">
                        <b-img v-bind:src="featuredImage.fileName == null ? noImage : require('@/assets/images/media/' + featuredImage.fileName)"
                               :alt="featuredImage.altText"
                               rounded
                               class="select-featured-image"
                               v-b-modal.modal-media
                               @click="selectImage" />
                        <b-button v-ripple.400="'rgba(186, 191, 199, 0.15)'"
                                  variant="relief-primary"
                                  size="sm"
                                  class="btn-icon rounded-circle select-image select-featured-image">
                            <feather-icon icon="Edit2Icon"
                                          class="select-featured-image"
                                          size="11" />
                        </b-button>
                    </div>
                    <b-button v-ripple.400="'rgba(186, 191, 199, 0.15)'"
                              variant="relief-secondary"
                              size="sm"
                              class="btn-icon rounded-circle remove-image remove-featured-image"
                              @click="removeImage">
                        <feather-icon icon="XIcon"
                                      class="remove-featured-image"
                                      size="11" />
                    </b-button>
                    <b-form-input type="text"
                                  hidden
                                  v-model="featuredImage.id"></b-form-input>
                </div>
            </b-card-actions>
            <b-card-actions title="Diğer Ayarlar"
                            action-collapse
                            collapsed>
                <b-form-checkbox v-model="postAddDto.IsShowSubPosts"
                                 name="check-button"
                                 switch
                                 inline>
                    Gönderi, diğer sayfalarda gösterilsin mi?
                </b-form-checkbox>
            </b-card-actions>
        </b-col>
    </b-row>
</template>

<script>
    import ModalMedia from '../../media/ModalMedia.vue';
    import UrlHelper from '@/helper/url-helper';
    import ToastificationContent from '@core/components/toastification/ToastificationContent.vue';
    import BCardActions from '@core/components/b-card-actions/BCardActions.vue';
    import AppCollapse from '@core/components/app-collapse/AppCollapse.vue';
    import AppCollapseItem from '@core/components/app-collapse/AppCollapseItem.vue';
    import { quillEditor } from 'vue-quill-editor';
    import 'quill/dist/quill.snow.css'
    import vSelect from 'vue-select';
    import { ValidationProvider, ValidationObserver, extend } from 'vee-validate';
    import { required } from '@validations';
    import {
        BRow, BCol, BBreadcrumb, BBreadcrumbItem, BSpinner, BButton, BDropdown, BDropdownItem, BCard, BCardBody, BForm, BFormGroup, BInputGroup, BFormInput,
        BInputGroupPrepend, BFormTextarea, BFormTags, BFormCheckbox, BCollapse, BImg, BTabs, BTab, BPagination, VBToggle, VBTooltip, BLink
    } from 'bootstrap-vue';
    import axios from 'axios';
    import Ripple from 'vue-ripple-directive';

    extend('required', {
        ...required,
        message: 'Lütfen gerekli bilgileri yazınız.'
    });

    export default {
        components: {
            ModalMedia,
            ToastificationContent,
            ValidationProvider,
            ValidationObserver,
            vSelect,
            quillEditor,
            BCardActions,
            AppCollapse,
            AppCollapseItem,
            BRow,
            BCol,
            BBreadcrumb,
            BBreadcrumbItem,
            BSpinner,
            BButton,
            BDropdown,
            BDropdownItem,
            BCard,
            BCardBody,
            BForm,
            BFormGroup,
            BInputGroup,
            BFormInput,
            BInputGroupPrepend,
            BFormCheckbox,
            BFormTextarea,
            BFormTags,
            BCollapse,
            BImg,
            BTabs,
            BTab,
            BPagination,
            BLink
        },
        directives: {
            'b-toggle': VBToggle,
            'b-tooltip': VBTooltip,
            Ripple,
        },
        data() {
            return {
                buttonDisabled: false,
                saveButtonVariant: 'primary',
                isTopPageSelectLoading: false,
                isSchemaPageSelectLoading: false,
                isSchemaArticleSelectLoading: false,
                nullPageMessage: 'Hiçbir temel sayfa bulunamadı.',
                nullSchemaMessage: 'Hiçbir şema türü bulunamadı.',
                breadcrumbs: [
                    {
                        text: 'Sayfalar',
                        to: { name: 'pages-basepage-list' }
                    },
                    {
                        text: 'Sayfa Ekle',
                        active: true,
                    },
                ],
                editorOption: {
                    placeholder: 'Sayfa İçeriği',
                    theme: 'snow'
                },
                required,
                isShowPostName: false,
                domainName: window.location.origin,
                parentPostName: '',
                isSlugEditActive: false,
                oldPostName: '',
                postAddDto: {
                    PostName: '',
                    Title: '',
                    Content: '',
                    BottomContent: '',
                    PostType: 'basepage',
                    PostStatus: null,
                    ParentId: '',
                    FeaturedImageId: '',
                    IsShowFeaturedImage: Boolean,
                    IsShowPage: Boolean,
                    IsShowSubPosts: Boolean,
                },
                pageSeoSettingAddDto: {
                    SeoTitle: '',
                    FocusKeyword: '',
                    SeoDescription: '',
                    CanonicalUrl: '',
                    IsRobotsNoIndex: Boolean,
                    IsRobotsNoFollow: Boolean,
                    IsRobotsNoArchive: Boolean,
                    IsRobotsNoImageIndex: Boolean,
                    IsRobotsNoSnippet: Boolean,
                    SchemaPageType: 0,
                    SchemaArticleType: 9,
                    OpenGraphImageId: '',
                    OpenGraphTitle: '',
                    OpenGraphDescription: '',
                    TwitterImageId: '',
                    TwitterTitle: '',
                    TwitterDescription: '',
                },
                topPosts: [],
                featuredImage: {
                    id: null,
                    fileName: null,
                    altText: null
                },
                isFeaturedImageChoose: false,
                keywords: [],
                noImage: require('@/assets/images/default/default-post-image.jpg'),
                openGraphImage: {
                    id: null,
                    fileName: null,
                    altText: null
                },
                twitterImage: {
                    id: null,
                    fileName: null,
                    altText: null
                },
                isOpenGraphImageChoose: false,
                isTwitterImageChoose: false,
                schemaPageTypes: [],
                schemaArticleTypes: []
            }
        },
        methods: {
            allTopPosts() {
                this.isTopPageSelectLoading = true;
                axios.get('/admin/post-alltopposts',
                    {
                        params: {
                            postId: null
                        }
                    })
                    .then((response) => {
                        if (response.data.PostListDto.ResultStatus === 0) {
                            this.topPosts = response.data.PostListDto.Data.Posts;
                        } else {
                            this.topPosts = [];
                            this.nullPageMessage = response.data.PostListDto.Message;
                        }
                        this.isTopPageSelectLoading = false
                    })
                    .catch((error) => {
                        this.$toast({
                            component: ToastificationContent,
                            props: {
                                variant: 'danger',
                                title: 'Hata Oluştu!',
                                icon: 'AlertOctagonIcon',
                                text: 'Ebeveyn sayfalar listenirken hata oluştu. Lütfen tekrar deneyiniz.',
                            }
                        })
                    });
            },
            getSchemaPageType() {
                this.isSchemaPageSelectLoading = true;
                axios.get('/admin/post-getschemapagetype')
                    .then((response) => {
                        if (response.data != null) {
                            this.schemaPageTypes = response.data;
                        } else {
                            this.schemaPageTypes = [];
                        }
                        this.isSchemaPageSelectLoading = false;
                    })
                    .catch((error) => {
                        this.$toast({
                            component: ToastificationContent,
                            props: {
                                variant: 'danger',
                                title: 'Hata Oluştu!',
                                icon: 'AlertOctagonIcon',
                                text: 'Hata oluştu. Lütfen tekrar deneyin.',
                            }
                        })
                    });
            },
            getSchemaArticleType() {
                this.isSchemaArticleSelectLoading = true;
                axios.get('/admin/post-getschemaarticletype')
                    .then((response) => {
                        if (response.data != null) {
                            this.schemaArticleTypes = response.data;
                        } else {
                            this.schemaArticleTypes = [];
                        }
                        this.isSchemaArticleSelectLoading = false;
                    })
                    .catch((error) => {
                        this.$toast({
                            component: ToastificationContent,
                            props: {
                                variant: 'danger',
                                title: 'Hata Oluştu!',
                                icon: 'AlertOctagonIcon',
                                text: 'Hata oluştu. Lütfen tekrar deneyin.',
                            }
                        })
                    });
            },
            postNameEdit() {
                this.oldPostName = this.postAddDto.PostName;
                var seoPostName = UrlHelper.friendlySEOUrl(this.postAddDto.PostName);
                this.postAddDto.PostName = seoPostName;
                this.isSlugEditActive = false;
            },
            postNameEditCancel() {
                this.isSlugEditActive = false;
                this.postAddDto.PostName = this.oldPostName;
            },
            changePostName() {
                if (!this.postAddDto.Title) {
                    this.isShowPostName = false;
                }
                else {
                    this.isShowPostName = true;
                    var seoPostName = UrlHelper.friendlySEOUrl(this.postAddDto.Title);
                    this.postAddDto.PostName = seoPostName;
                }
            },
            imageChange(id, name, altText) {
                if (this.isFeaturedImageChoose == true) {
                    this.featuredImage.id = id;
                    this.featuredImage.fileName = name;
                    this.featuredImage.altText = altText;
                } else if (this.isOpenGraphImageChoose == true) {
                    this.openGraphImage.id = id;
                    this.openGraphImage.fileName = name;
                    this.openGraphImage.altText = altText;
                } else if (this.isTwitterImageChoose == true) {
                    this.twitterImage.id = id;
                    this.twitterImage.fileName = name;
                    this.twitterImage.altText = altText;
                }
                this.isFeaturedImageChoose = false;
                this.isOpenGraphImageChoose = false;
                this.isTwitterImageChoose = false;
            },
            selectImage: function (e) {
                if (e.target._prevClass.includes('select-featured-image')) {
                    this.isFeaturedImageChoose = true;
                } else if (e.target._prevClass.includes('select-opengraph-image')) {
                    this.isOpenGraphImageChoose = true;
                } else if (e.target._prevClass.includes('select-twitter-image')) {
                    this.isTwitterImageChoose = true;
                }
            },
            removeImage: function (e) {
                if (e.target._prevClass.includes('remove-featured-image')) {
                    this.featuredImage.id = null;
                    this.featuredImage.fileName = null;
                    this.featuredImage.altText = null;
                } else if (e.target._prevClass.includes('remove-opengraph-image')) {
                    this.openGraphImage.id = null;
                    this.openGraphImage.fileName = null;
                    this.openGraphImage.altText = null;
                } else if (e.target._prevClass.includes('remove-twitter-image')) {
                    this.twitterImage.id = null;
                    this.twitterImage.fileName = null;
                    this.twitterImage.altText = null;
                }
            },           
            validationForm: function (e) {                
                this.postAddDto.FeaturedImageId = this.featuredImage.id;
                this.pageSeoSettingAddDto.FocusKeyword = this.keywords.toString();
                this.pageSeoSettingAddDto.OpenGraphImageId = this.openGraphImage.id;
                this.pageSeoSettingAddDto.TwitterImageId = this.twitterImage.id;
                console.log(this.pageSeoSettingAddDto.SchemaPageType);
                console.log(this.pageSeoSettingAddDto.SchemaArticleType);
                if (e.target.id == "save") {
                    this.postAddDto.PostStatus = "publish";
                }
                else if (e.target.id == "draft") {
                    this.postAddDto.PostStatus = "draft";
                }
                this.$refs.pageAddForm.validate().then(success => {
                    if (success) {
                        this.buttonDisabled = true;
                        this.saveButtonVariant = 'outline-secondary';
                        axios.post('/admin/post-new',
                            {
                                PostAddDto: this.postAddDto,
                                SeoObjectSettingAddDto: this.pageSeoSettingAddDto
                            })
                            .then((response) => {
                                console.log(response.data)
                                if (response.data.PostDto.ResultStatus === 0) {
                                    if (this.$can('update', 'Basepage')) {
                                        this.$router.push({ name: 'pages-basepage-edit', query: { edit: response.data.PostDto.Data.Post.Id } });
                                    } else {
                                        this.$router.push({ name: 'pages-basepage-list' });
                                    }
                                    this.$toast({
                                        component: ToastificationContent,
                                        props: {
                                            variant: 'success',
                                            title: 'Başarılı İşlem!',
                                            icon: 'CheckIcon',
                                            text: response.data.PostDto.Message
                                        }
                                    });
                                }
                                else {
                                    this.$toast({
                                        component: ToastificationContent,
                                        props: {
                                            variant: 'danger',
                                            title: 'Başarısız İşlem!',
                                            icon: 'AlertOctagonIcon',
                                            text: response.data.PostDto.Message
                                        },
                                    });
                                }
                            })
                            .catch((error) => {
                                console.log(error);
                                console.log(error.request);
                                this.$toast({
                                    component: ToastificationContent,
                                    props: {
                                        variant: 'danger',
                                        title: 'Hata Oluştu!',
                                        icon: 'AlertOctagonIcon',
                                        text: 'Hata oluştu. Lütfen tekrar deneyiniz.',
                                    },
                                })
                            });
                    }
                })
            },
        },
        computed: {
        },
        mounted() {
            this.allTopPosts();
            this.getSchemaPageType();
            this.getSchemaArticleType();
        }
    }
</script>

<style lang="scss">
    @import '@core/scss/vue/libs/vue-select.scss';

    .ql-editor {
        min-height: 500px !important;
    }

    .card-border {
        margin-top: 10px;
        border: 1px solid rgb(34 41 47 / 10%);
        border-radius: 5px;
    }

    .image-preview {
        width: 100%;
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