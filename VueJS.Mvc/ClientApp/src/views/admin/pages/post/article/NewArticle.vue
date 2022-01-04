<template>
    <b-row class="content-header">
        <b-col class="content-header-left mb-2"
               cols="12"
               md="6">
            <b-row class="breadcrumbs-top">
                <b-col cols="12">
                    <h2 class="content-header-title float-left pr-1 mb-0">
                        Makale Ekle
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
            <b-button id="draft"
                      variant="outline-primary"
                      class="mr-1"
                      size="sm"
                      type="submit"
                      @click.prevent="validationForm">
                Taslak Olarak Kaydet
            </b-button>
            <b-button id="save"
                      variant="primary"
                      type="submit"
                      @click.prevent="validationForm">
                Yayınla
            </b-button>
        </b-col>
        <modal-media v-bind:show="modalShow"
                     @changeImage="imageChange"
                     ref="modalMedia"></modal-media>
        <b-col md="12"
               lg="8">
            <b-card>
                <validation-observer ref="articleAddForm">
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
                                                      placeholder="Başlık" />
                                        <small class="text-danger">{{ errors[0] }}</small>
                                    </validation-provider>
                                </b-form-group>
                                <quill-editor v-model="postAddDto.Content"
                                              :options="editorOption" />
                            </b-col>
                        </b-row>
                    </b-form>
                </validation-observer>
            </b-card>
            <b-card-actions title="SEO Ayarları"
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
                                                      v-model="articleSeoSettingAddDto.SeoTitle"
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
                                                         v-model="articleSeoSettingAddDto.SeoDescription"
                                                         placeholder="Meta Açıklama"
                                                         rows="3" />
                                    </b-form-group>
                                    <app-collapse>
                                        <app-collapse-item title="Gelişmiş">
                                            <b-form-group label-for="CanonicalUrl"
                                                          description="Geçerli makale ile benzer içeriğe sahip olan makalenin linkini giriniz.">
                                                <b-form-input id="CanonicalUrl"
                                                              v-model="articleSeoSettingAddDto.CanonicalUrl"
                                                              type="text"
                                                              placeholder="Benzer Link" />
                                            </b-form-group>
                                            <b-form-group>
                                                <b-form-checkbox v-model="articleSeoSettingAddDto.IsRobotsNoIndex"
                                                                 name="check-button"
                                                                 switch
                                                                 inline>
                                                    Arama motorlarının bu terimi arama sonuçlarında göstermesini istiyor musunuz?
                                                </b-form-checkbox>
                                            </b-form-group>
                                            <b-form-group>
                                                <b-form-checkbox v-model="articleSeoSettingAddDto.IsRobotsNoFollow"
                                                                 name="check-button"
                                                                 switch
                                                                 inline>
                                                    Arama motorları bu sayfa üzerindeki bağlantıları(linkleri) izlemeli mi?
                                                </b-form-checkbox>
                                            </b-form-group>
                                            <b-form-group>
                                                <b-form-checkbox v-model="articleSeoSettingAddDto.IsRobotsNoArchive"
                                                                 name="check-button"
                                                                 switch
                                                                 inline>
                                                    Arama motorları bu sayfanın önbelleğini almalı mı?
                                                </b-form-checkbox>
                                            </b-form-group>
                                            <b-form-group>
                                                <b-form-checkbox v-model="articleSeoSettingAddDto.IsRobotsNoImageIndex"
                                                                 name="check-button"
                                                                 switch
                                                                 inline>
                                                    Arama motorları bu sayfa üzerindeki resimleri indexlemeli mi?
                                                </b-form-checkbox>
                                            </b-form-group>
                                            <b-form-group>
                                                <b-form-checkbox v-model="articleSeoSettingAddDto.IsRobotsNoSnippet"
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
                            <v-select id="schnemaPageType"
                                      v-model="articleSeoSettingAddDto.SchemaPageType"
                                      :options="schnemaPageTypes"
                                      label="Name"
                                      :reduce="(option) => option.Id"
                                      placeholder="Sayfa Türü Seçiniz..."
                                      @input="changePageType()" />
                        </b-form-group>
                        <b-form-group label="Makale Türü">
                            <v-select id="schnemaArticleType"
                                      v-model="selectedArticleType"
                                      :options="schnemaArticleTypes"
                                      label="Name"
                                      :reduce="(option) => option.Id"
                                      placeholder="Makale Türü Seçiniz..."
                                      @input="changeArticleType" />
                        </b-form-group>
                    </b-tab>
                    <b-tab title="Sosyal Medya">
                        <app-collapse>
                            <app-collapse-item title="Sosyal Medya">
                                <b-row class="kb-search-content-info match-height">
                                    <b-col lg="4"
                                           md="4"
                                           sm="6"
                                           class="image-thumb ml-1">
                                        <b-img rounded
                                               v-bind:src="openGraphImage.fileName == null ? noImage : require('@/assets/images/media/' + openGraphImage.fileName)"
                                               :alt="openGraphImage.altText" />
                                    </b-col>
                                    <b-col lg="3"
                                           md="3"
                                           sm="6">
                                        <b-button id="selectOpenGraphImage"
                                                  v-ripple.400="'rgba(255, 255, 255, 0.15)'"
                                                  variant="primary"
                                                  size="sm"
                                                  class="mb-75 mr-75"
                                                  v-b-modal.modal-media
                                                  @click="selectImage">
                                            Resim Seç
                                        </b-button>
                                        <!--/ upload button -->
                                        <!-- reset -->
                                        <b-button id="removeOpenGraphImage"
                                                  v-ripple.400="'rgba(186, 191, 199, 0.15)'"
                                                  variant="outline-secondary"
                                                  size="sm"
                                                  class="mb-75 mr-75"
                                                  @click="removeImage">
                                            Resmi Kaldır
                                        </b-button>
                                        <b-form-input type="text"
                                                      hidden
                                                      v-model="openGraphImage.id"></b-form-input>
                                    </b-col>
                                </b-row>
                                <b-form-group class="mt-1">
                                    <b-form-input id="OpenGraphTitle"
                                                  v-model="articleSeoSettingAddDto.OpenGraphTitle"
                                                  type="text"
                                                  placeholder="Sosyal Medya Başlığı" />
                                </b-form-group>
                                <b-form-group>
                                    <b-form-textarea id="OpenGraphDescription"
                                                     v-model="articleSeoSettingAddDto.OpenGraphDescription"
                                                     placeholder="Sosyal Medya Açıklaması"
                                                     rows="3" />
                                </b-form-group>
                            </app-collapse-item>
                            <app-collapse-item title="Twitter">
                                <b-row class="kb-search-content-info match-height">
                                    <b-col lg="4"
                                           md="4"
                                           sm="6"
                                           class="image-thumb ml-1">
                                        <b-img rounded
                                               v-bind:src="twitterImage.fileName == null ? noImage : require('@/assets/images/media/' + twitterImage.fileName)"
                                               :alt="twitterImage.altText" />
                                    </b-col>
                                    <b-col lg="3"
                                           md="3"
                                           sm="6">
                                        <b-button id="selectTwitterImage"
                                                  v-ripple.400="'rgba(255, 255, 255, 0.15)'"
                                                  variant="primary"
                                                  size="sm"
                                                  class="mb-75 mr-75"
                                                  v-b-modal.modal-media
                                                  @click="selectImage">
                                            Resim Seç
                                        </b-button>
                                        <!--/ upload button -->
                                        <!-- reset -->
                                        <b-button id="removeTwitterImage"
                                                  v-ripple.400="'rgba(186, 191, 199, 0.15)'"
                                                  variant="outline-secondary"
                                                  size="sm"
                                                  class="mb-75 mr-75"
                                                  @click="removeImage">
                                            Resmi Kaldır
                                        </b-button>
                                        <b-form-input type="text"
                                                      hidden
                                                      v-model="twitterImage.id"></b-form-input>
                                    </b-col>
                                </b-row>
                                <b-form-group class="mt-1">
                                    <b-form-input id="TwitterTitle"
                                                  v-model="articleSeoSettingAddDto.TwitterTitle"
                                                  type="text"
                                                  placeholder="Twitter Başlığı" />
                                </b-form-group>
                                <b-form-group>
                                    <b-form-textarea id="TwitterDescription"
                                                     v-model="articleSeoSettingAddDto.TwitterDescription"
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
            <b-card title="Kategori">
                <b-form-group>
                    <v-select id="categoryList"
                              v-model="selectedCategory"
                              :options="categories"
                              label="Name"
                              :reduce="(option) => option.Id"
                              placeholder="Kategori Seçiniz..."
                              taggable 
                              multiple
                              @input="changeCategory" />
                </b-form-group>
                <b-button v-b-toggle.new-category
                          size="sm"
                          variant="flat-primary">
                    + Yeni Kategori Ekle
                </b-button>
                <b-collapse id="new-category">
                    <b-card class="card-border">
                        <validation-observer ref="categoryAddForm">
                            <b-form>
                                <b-form-group>
                                    <validation-provider #default="{ errors }"
                                                         name="categoryName"
                                                         vid="categoryName"
                                                         rules="required">
                                        <b-form-input v-model="categoryAddDto.Name"
                                                      :state="errors.length > 0 ? false:null"
                                                      type="text"
                                                      size="sm"
                                                      placeholder="Kategori Adı" />
                                        <small class="text-danger">{{ errors[0] }}</small>
                                    </validation-provider>
                                </b-form-group>
                                <b-form-group>
                                    <v-select id="parentTerms"
                                              v-model="selectedParentTerm"
                                              :options="allParentTerms"
                                              label="Name"
                                              :reduce="(option) => option.Id"
                                              class="select-size-sm"
                                              placeholder="— Üst Kategori —"
                                              @input="changeParentTerm" />
                                </b-form-group>
                                <b-button variant="outline-primary"
                                          size="sm"
                                          @click.prevent="validationFormCategory">
                                    Ekle
                                </b-button>
                            </b-form>
                        </validation-observer>
                    </b-card>
                </b-collapse>
            </b-card>
            <b-card title="Etiket">
                <b-form-group>
                    <v-select v-model="selectedTag"
                              :options="tags"
                              label="Name"
                              :reduce="(option) => option.Id"
                              placeholder="Etiket Seçiniz..."
                              multiple
                              @input="changeTag" />
                </b-form-group>
                <b-button v-b-toggle.new-tag
                          size="sm"
                          variant="flat-primary">
                    + Yeni Etiket Ekle
                </b-button>
                <b-collapse id="new-tag">
                    <b-card class="card-border">
                        <validation-observer ref="tagAddForm">
                            <b-form>
                                <b-form-group>
                                    <validation-provider #default="{ errors }"
                                                         name="tagName"
                                                         vid="tagName"
                                                         rules="required">
                                        <b-form-input v-model="tagAddDto.Name"
                                                      :state="errors.length > 0 ? false:null"
                                                      type="text"
                                                      size="sm"
                                                      placeholder="Etiket Adı" />
                                        <small class="text-danger">{{ errors[0] }}</small>
                                    </validation-provider>
                                </b-form-group>
                                <b-button variant="outline-primary"
                                          size="sm"
                                          @click.prevent="validationFormTag">
                                    Ekle
                                </b-button>
                            </b-form>
                        </validation-observer>
                    </b-card>
                </b-collapse>
            </b-card>
            <b-card-actions title="Resim"
                            action-collapse
                            collapsed>
                <b-form-group>
                    <b-form-checkbox v-model="postAddDto.IsShowFeaturedImage"
                                     name="check-button"
                                     switch
                                     inline>
                        Görsel yazıda gösterilsin mi?
                    </b-form-checkbox>
                </b-form-group>
                <div class="image-thumb">
                    <b-img rounded
                           v-bind:src="featuredImage.fileName == null ? noImage : require('@/assets/images/media/' + featuredImage.fileName)"
                           :alt="featuredImage.altText" />
                </div>
                <div class="mt-1">
                    <b-button id="selectFeaturedImage"
                              v-ripple.400="'rgba(255, 255, 255, 0.15)'"
                              variant="primary"
                              size="sm"
                              class="mb-75 mr-75"
                              v-b-modal.modal-media
                              @click="selectImage">
                        Resim Seç
                    </b-button>
                    <!--/ upload button -->
                    <!-- reset -->
                    <b-button id="removeFeaturedImage"
                              v-ripple.400="'rgba(186, 191, 199, 0.15)'"
                              variant="outline-secondary"
                              size="sm"
                              class="mb-75 mr-75"
                              @click="removeImage">
                        Resmi Kaldır
                    </b-button>
                    <b-form-input type="text"
                                  hidden
                                  v-model="featuredImage.id"></b-form-input>
                </div>
            </b-card-actions>
            <b-card-actions title="Diğer Ayarlar"
                            action-collapse
                            collapsed>
                <b-form-checkbox v-model="postAddDto.CommentStatus"
                                 name="check-button"
                                 switch
                                 inline>
                    Bu yazı için yorumlar açılsın mı?
                </b-form-checkbox>
            </b-card-actions>
        </b-col>
    </b-row>
</template>

<script>
    import 'quill/dist/quill.snow.css'

    import { ValidationProvider, ValidationObserver, extend } from 'vee-validate';
    import required from '@validations';
    import {
        BBreadcrumb, BBreadcrumbItem, BCollapse, BSpinner, BImg, BTabs, BTab, BFormTags, BFormCheckbox, BButton, BCard, BCardBody, BCardTitle, BRow, BCol, BForm, BFormSelect, BFormGroup, BFormTextarea, BPagination, BInputGroup, BFormInput, BInputGroupPrepend, VBToggle, VBTooltip, BLink
    } from 'bootstrap-vue';
    import BCardActions from '@core/components/b-card-actions/BCardActions.vue';
    import axios from 'axios';
    import ToastificationContent from '@core/components/toastification/ToastificationContent.vue';
    import vSelect from 'vue-select';
    import Ripple from 'vue-ripple-directive';
    import { quillEditor } from 'vue-quill-editor';
    import ModalMedia from '../../media/ModalMedia.vue';
    import AppCollapse from '@core/components/app-collapse/AppCollapse.vue';
    import AppCollapseItem from '@core/components/app-collapse/AppCollapseItem.vue';

    extend('required', {
        ...required,
        message: 'Lütfen gerekli bilgileri yazınız.'
    });

    export default {
        components: {
            BBreadcrumb,
            BBreadcrumbItem,
            BCollapse,
            AppCollapse,
            AppCollapseItem,
            quillEditor,
            ModalMedia,
            BCardActions,
            BSpinner,
            BImg,
            BTabs,
            BTab,
            BFormTags,
            BCardTitle,
            BForm,
            BFormSelect,
            BButton,
            BFormCheckbox,
            BFormTextarea,
            BCard,
            BRow,
            BCol,
            BCardBody,
            BFormGroup,
            BPagination,
            BInputGroup,
            BFormInput,
            BInputGroupPrepend,
            ToastificationContent,
            ValidationProvider,
            ValidationObserver,
            vSelect,
            BLink
        },
        directives: {
            'b-toggle': VBToggle,
            'b-tooltip': VBTooltip,
            Ripple,
        },
        data() {
            return {
                breadcrumbs: [
                    {
                        text: 'Makaleler',
                        to: { name: 'pages-article-list' }
                    },
                    {
                        text: 'Makale Ekle',
                        active: true,
                    },
                ],
                editorOption: {
                    placeholder: 'Makale İçeriği',
                    theme: 'snow'
                },
                required,
                isSpinnerShow: true,
                title: '',
                postAddDto: {
                    Title: '',
                    Content: '',
                    PostType: 'article',
                    PostStatus: null,
                    CommentStatus: Boolean,
                    FeaturedImageId: '',
                    IsShowFeaturedImage: Boolean
                },
                categoryAddDto: {
                    Name: '',
                    ParentId: null,
                    TermType: 'category'
                },
                tagAddDto: {
                    Name: '',
                    TermType: 'tag'
                },
                articleSeoSettingAddDto: {
                    SeoTitle: '',
                    FocusKeyword: '',
                    SeoDescription: '',
                    CanonicalUrl: '',
                    IsRobotsNoIndex: Boolean,
                    IsRobotsNoFollow: Boolean,
                    IsRobotsNoArchive: Boolean,
                    IsRobotsNoImageIndex: Boolean,
                    IsRobotsNoSnippet: Boolean,
                    SchemaPageType: '',
                    SchemaArticleType: '',
                    OpenGraphImageId: '',
                    OpenGraphTitle: '',
                    OpenGraphDescription: '',
                    TwitterImageId: '',
                    TwitterTitle: '',
                    TwitterDescription: '',
                },
                termSeoSettingAddDto: {
                    SeoTitle: ''
                },
                postTermAddDto: {
                    PostId: '',
                    TermId: '',
                    TermType: ''
                },
                categories: [],
                selectedCategory: [],
                allParentTerms: [],
                selectedParentTerm: '',
                selectedParentTermValue: null,
                categoryName: '',
                tags: [],
                selectedTag: [],
                tagName: '',
                featuredImage: {
                    id: null,
                    fileName: null,
                    altText: null
                },
                isFeaturedImageChoose: false,
                keywords: [],
                modalShow: false,
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
                schnemaPageTypes: [],
                schnemaArticleTypes: [],
                selectedPageType: { Id: '0', Name: 'Sayfalar için varsayılan(Web sayfası)' },
                selectedArticleType: { Id: '0', Name: 'Yazılar için varsayılan(Makale)' }
            }
        },
        methods: {
            allCategories() {
                axios.get('/admin/term/allterms', {
                    params: {
                        term_type: 'category'
                    }
                })
                    .then((response) => {
                        if (response.data.ResultStatus === 0) {
                            this.categories = response.data.Terms;
                            this.allParentTerms = response.data.Terms;
                        }
                    })
                    .catch((error) => {
                        this.$toast({
                            component: ToastificationContent,
                            props: {
                                variant: 'danger',
                                title: 'Hata Oluştu!',
                                icon: 'AlertOctagonIcon',
                                text: 'Terimler listenirken hata oluştu. Lütfen tekrar deneyiniz.',
                            }
                        })
                    });
            },
            allTags() {
                axios.get('/admin/term/allterms', {
                    params: {
                        term_type: 'tag'
                    }
                })
                    .then((response) => {
                        if (response.data.ResultStatus === 0) {
                            this.tags = response.data.Terms;
                        }
                    })
                    .catch((error) => {
                        this.$toast({
                            component: ToastificationContent,
                            props: {
                                variant: 'danger',
                                title: 'Hata Oluştu!',
                                icon: 'AlertOctagonIcon',
                                text: 'Terimler listenirken hata oluştu. Lütfen tekrar deneyiniz.',
                            }
                        })
                    });
            },
            changeCategory(value) {
                this.selectedCategory = value;
            },
            changeParentTerm(value) {
                this.categoryAddDto.ParentId = value;
            },
            changeTag(value) {
                this.selectedTag = value;
            },
            addNewTerm: function (e) {
                if (e.target.id == "newCategory") {

                } else if (e.target.id == "newTag") {

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
                if (e.target.id == "selectFeaturedImage") {
                    this.isFeaturedImageChoose = true;
                } else if (e.target.id == "selectOpenGraphImage") {
                    this.isOpenGraphImageChoose = true;
                } else if (e.target.id == "selectTwitterImage") {
                    this.isTwitterImageChoose = true;
                }
                this.modalShow = true;
            },
            removeImage: function (e) {
                if (e.target.id == "removeFeaturedImage") {
                    this.featuredImage.id = null;
                    this.featuredImage.fileName = null;
                    this.featuredImage.altText = null;
                } else if (e.target.id == "removeOpenGraphImage") {
                    this.openGraphImage.id = null;
                    this.openGraphImage.fileName = null;
                    this.openGraphImage.altText = null;
                } else if (e.target.id == "removeTwitterImage") {
                    this.twitterImage.id = null;
                    this.twitterImage.fileName = null;
                    this.twitterImage.altText = null;
                }
            },
            getSchnemaPageType() {
                axios.get('/admin/post/getschnemapagetype')
                    .then((response) => {
                        this.schnemaPageTypes = response.data;
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
            getSchnemaArticleType() {
                axios.get('/admin/post/getschnemaarticletype')
                    .then((response) => {
                        this.schnemaArticleTypes = response.data;
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
            changePageType(value) {
                this.articleSeoSettingAddDto.SchemaPageType = value;
            },
            changeArticleType(value) {
                this.articleSeoSettingAddDto.SchemaArticleType = value;
            },
            validationForm: function (e) {
                this.postAddDto.FeaturedImageId = this.featuredImage.id;
                this.articleSeoSettingAddDto.FocusKeyword = this.keywords.toString();
                this.articleSeoSettingAddDto.OpenGraphImageId = this.openGraphImage.id;
                this.articleSeoSettingAddDto.TwitterImageId = this.twitterImage.id;
                this.articleSeoSettingAddDto.SchemaPageType = this.selectedPageType.Id;
                this.articleSeoSettingAddDto.SchemaArticleType = this.selectedArticleType.Id;
                if (e.target.id == "save") {
                    this.postAddDto.PostStatus = "publish";
                }
                else if (e.target.id == "draft") {
                    this.postAddDto.PostStatus = "draft";
                }
                this.$refs.articleAddForm.validate().then(success => {
                    if (success) {
                        axios.post('/admin/post/new',
                            {
                                PostAddDto: this.postAddDto,
                                SeoObjectSettingAddDto: this.articleSeoSettingAddDto
                            })
                            .then((response) => {
                                if (response.data.PostDto.ResultStatus === 0) {

                                    this.postTermAddDto.PostId = response.data.PostDto.Post.Id;
                                    if (this.selectedCategory.length > 0) {
                                        for (var i = 0; i < this.selectedCategory.length; i++) {
                                            this.postTermAddDto.TermId = this.selectedCategory[i];
                                            this.postTermAddDto.TermType = "category";

                                            axios.post('/admin/term/newpostterm', {
                                                PostTermAddDto: this.postTermAddDto
                                            });
                                        }
                                    }

                                    if (this.selectedTag.length > 0) {
                                        for (var i = 0; i < this.selectedTag.length; i++) {
                                            this.postTermAddDto.TermId = this.selectedTag[i];
                                            this.postTermAddDto.TermType = "tag";

                                            axios.post('/admin/term/newpostterm', {
                                                PostTermAddDto: this.postTermAddDto
                                            });
                                        }
                                    }

                                    this.$router.push({ name: 'pages-post-edit', query: { edit: response.data.PostDto.Post.Id } })

                                    this.$toast({
                                        component: ToastificationContent,
                                        props: {
                                            variant: 'success',
                                            title: 'Başarılı İşlem!',
                                            icon: 'CheckIcon',
                                            text: response.data.PostDto.Message
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
                                            text: response.data.PostDto.Message
                                        },
                                    })
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
            validationFormCategory() {
                this.$refs.categoryAddForm.validate().then(success => {
                    if (success) {
                        axios.post('/admin/term/new',
                            {
                                TermAddDto: this.categoryAddDto,
                                SeoObjectSettingAddDto: this.termSeoSettingAddDto
                            })
                            .then((response) => {
                                if (response.data.TermDto.ResultStatus === 0) {
                                    this.$toast({
                                        component: ToastificationContent,
                                        props: {
                                            variant: 'success',
                                            title: 'Başarılı İşlem!',
                                            icon: 'CheckIcon',
                                            text: response.data.TermDto.Message
                                        }
                                    })
                                    this.allCategories();
                                    this.selectedCategory.push(response.data.TermDto.Term.Id)
                                }
                                else {
                                    this.$toast({
                                        component: ToastificationContent,
                                        props: {
                                            variant: 'danger',
                                            title: 'Başarısız İşlem!',
                                            icon: 'AlertOctagonIcon',
                                            text: response.data.TermDto.Message
                                        },
                                    })
                                }
                            })
                            .catch((error) => {
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
            validationFormTag() {
                this.$refs.tagAddForm.validate().then(success => {
                    if (success) {
                        axios.post('/admin/term/new',
                            {
                                TermAddDto: this.tagAddDto,
                                SeoObjectSettingAddDto: this.termSeoSettingAddDto
                            })
                            .then((response) => {
                                if (response.data.TermDto.ResultStatus === 0) {
                                    this.$toast({
                                        component: ToastificationContent,
                                        props: {
                                            variant: 'success',
                                            title: 'Başarılı İşlem!',
                                            icon: 'CheckIcon',
                                            text: response.data.TermDto.Message
                                        }
                                    })
                                    this.allTags();
                                    this.selectedTag.push(response.data.TermDto.Term.Id)
                                }
                                else {
                                    this.$toast({
                                        component: ToastificationContent,
                                        props: {
                                            variant: 'danger',
                                            title: 'Başarısız İşlem!',
                                            icon: 'AlertOctagonIcon',
                                            text: response.data.TermDto.Message
                                        },
                                    })
                                }
                            })
                            .catch((error) => {
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
            singleDeleteData(id, name) {
                this.$swal({
                    title: 'Silmek istediğinize emin misiniz?',
                    text: name + " adlı makale kalıcı olarak silinecektir?",
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
                        axios.post('/admin/term/delete?term=' + id)
                            .then((response) => {
                                if (response.data.ResultStatus === 0) {
                                    this.$toast({
                                        component: ToastificationContent,
                                        props: {
                                            variant: 'success',
                                            title: 'Başarılı İşlem!',
                                            icon: 'CheckIcon',
                                            text: response.data.Message
                                        }
                                    })
                                    this.getAllData();
                                }
                                else {
                                    this.$toast({
                                        component: ToastificationContent,
                                        props: {
                                            variant: 'danger',
                                            title: 'Başarısız İşlem!',
                                            icon: 'AlertOctagonIcon',
                                            text: response.data.Message
                                        },
                                    })
                                }
                            })
                            .catch((error) => {
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
            this.allCategories();
            this.allTags();
            this.getSchnemaPageType();
            this.getSchnemaArticleType();
            console.log(this.selectedTag);
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

    .image-thumb {
        width: 100%;
        height: 200px;
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
</style>