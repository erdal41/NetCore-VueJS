<template>
    <b-row class="content-header">
        <modal-media v-bind:show="modalShow"
                     @changeImage="imageChange"
                     ref="modalMedia"></modal-media>
        <b-col class="content-header-left mb-2"
               cols="12"
               md="7">
            <h2 class="content-header-title float-left pr-1 mb-0">
                {{ pageTitle }}
            </h2>
            <div class="breadcrumb-wrapper">
                <b-breadcrumb>
                    <b-breadcrumb-item to="/">
                        <feather-icon icon="HomeIcon"
                                      size="16"
                                      class="align-text-top" />
                    </b-breadcrumb-item>
                    <b-breadcrumb-item v-if="isParent == true"
                                       :to="{ name: 'pages-page-list' }">Sayfalar</b-breadcrumb-item>
                    <b-breadcrumb-item v-else
                                       :to="{ name: 'pages-article-list' }">Makaleler</b-breadcrumb-item>
                    <b-breadcrumb-item active>Düzenle</b-breadcrumb-item>
                    <b-button v-if="postUpdateDto.PostType == '0'"
                              v-b-tooltip.hover
                              :title="tooltipText"
                              v-ripple.400="'rgba(113, 102, 240, 0.15)'"
                              variant="outline-primary"
                              size="sm"
                              :to="{ name:'pages-tag-list'}"
                              class=" ml-1">Yeni Ekle</b-button>
                    <b-button v-else-if="postUpdateDto.PostType == '1'"
                              v-b-tooltip.hover
                              :title="tooltipText"
                              v-ripple.400="'rgba(113, 102, 240, 0.15)'"
                              variant="outline-primary"
                              size="sm"
                              :to="{ name:'pages-article-add'}"
                              class=" ml-1">Yeni Ekle</b-button>

                    <b-button v-else-if="postUpdateDto.PostType == '5'"
                              v-b-tooltip.hover
                              :title="tooltipText"
                              v-ripple.400="'rgba(113, 102, 240, 0.15)'"
                              variant="outline-primary"
                              size="sm"
                              :to="{ name:'pages-category-list'}"
                              class=" ml-1">Yeni Ekle</b-button>
                </b-breadcrumb>
            </div>
        </b-col>
        <b-col class="content-header-right text-md-right d-md-block d-none mb-1"
               md="5"
               cols="12">
            <b-button id="draft"
                      variant="flat-danger"
                      class="mr-1"
                      size="sm"
                      type="button">
                Çöpe Taşı
            </b-button>
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
                Güncelle
            </b-button>
        </b-col>
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
                                                      v-model="postUpdateDto.Title"
                                                      :state="errors.length > 0 ? false:null"
                                                      type="text"
                                                      placeholder="Başlık" />
                                        <small class="text-danger">{{ errors[0] }}</small>
                                    </validation-provider>

                                </b-form-group>
                                <b-form-group>
                                    <span class="small">Gönderi linki: </span><a class="small" :href="domainName + '/' + postUpdateDto.PostName">{{ domainName }}/<span v-show="isSlugEditActive == false">{{  postUpdateDto.PostName }}</span></a>
                                    <b-button v-show="isSlugEditActive == false"
                                              v-b-tooltip.hover
                                              title="Gönderinin linkini değiştirmenizi sağlar."
                                              v-ripple.400="'rgba(113, 102, 240, 0.15)'"
                                              variant="outline-primary"
                                              size="sm"
                                              class="ml-1"
                                              @click="postNameEdit">
                                        Düzenle
                                    </b-button>
                                    <div v-show="isSlugEditActive == true"
                                         class="card-border p-1">
                                        <b-form-input type="text"
                                                      v-model="postUpdateDto.PostName"
                                                      placeholder="Gönderi Linki"
                                                      size="sm">
                                        </b-form-input>
                                        <b-button variant="primary"
                                                  class="mt-1"
                                                  size="sm"
                                                  @click="isSlugEditActive = !isSlugEditActive">
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
                                <b-form-group>
                                    <quill-editor v-model="postUpdateDto.Content"
                                                  :options="editorOption" />
                                </b-form-group>
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
                                                      v-model="seoObjectSettingUpdateDto.SeoTitle"
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
                                                         v-model="seoObjectSettingUpdateDto.SeoDescription"
                                                         placeholder="Meta Açıklama"
                                                         rows="3" />
                                    </b-form-group>
                                    <app-collapse>
                                        <app-collapse-item title="Gelişmiş">
                                            <b-form-group label-for="CanonicalUrl"
                                                          description="Geçerli makale ile benzer içeriğe sahip olan makalenin linkini giriniz.">
                                                <b-form-input id="CanonicalUrl"
                                                              v-model="seoObjectSettingUpdateDto.CanonicalUrl"
                                                              type="text"
                                                              placeholder="Benzer Link" />
                                            </b-form-group>
                                            <b-form-group>
                                                <b-form-checkbox v-model="seoObjectSettingUpdateDto.IsRobotsNoIndex"
                                                                 name="check-button"
                                                                 switch
                                                                 inline>
                                                    Arama motorlarının bu terimi arama sonuçlarında göstermesini istiyor musunuz?
                                                </b-form-checkbox>
                                            </b-form-group>
                                            <b-form-group>
                                                <b-form-checkbox v-model="seoObjectSettingUpdateDto.IsRobotsNoFollow"
                                                                 name="check-button"
                                                                 switch
                                                                 inline>
                                                    Arama motorları bu sayfa üzerindeki bağlantıları(linkleri) izlemeli mi?
                                                </b-form-checkbox>
                                            </b-form-group>
                                            <b-form-group>
                                                <b-form-checkbox v-model="seoObjectSettingUpdateDto.IsRobotsNoArchive"
                                                                 name="check-button"
                                                                 switch
                                                                 inline>
                                                    Arama motorları bu sayfanın önbelleğini almalı mı?
                                                </b-form-checkbox>
                                            </b-form-group>
                                            <b-form-group>
                                                <b-form-checkbox v-model="seoObjectSettingUpdateDto.IsRobotsNoImageIndex"
                                                                 name="check-button"
                                                                 switch
                                                                 inline>
                                                    Arama motorları bu sayfa üzerindeki resimleri indexlemeli mi?
                                                </b-form-checkbox>
                                            </b-form-group>
                                            <b-form-group>
                                                <b-form-checkbox v-model="seoObjectSettingUpdateDto.IsRobotsNoSnippet"
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
                            <v-select v-model="seoObjectSettingUpdateDto.SchemaPageType"
                                      :options="schnemaPageTypes"
                                      label="Name"
                                      :reduce="(option) => option.Id"
                                      :clearable="false"
                                      @input="changePageType" />
                        </b-form-group>
                        <b-form-group label="Makale Türü">
                            <v-select v-model="seoObjectSettingUpdateDto.SchemaArticleType"
                                      :options="schnemaArticleTypes"
                                      label="Name"
                                      :reduce="(option) => option.Id"
                                      :clearable="false"
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
                                                  v-model="seoObjectSettingUpdateDto.OpenGraphTitle"
                                                  type="text"
                                                  placeholder="Sosyal Medya Başlığı" />
                                </b-form-group>
                                <b-form-group>
                                    <b-form-textarea id="OpenGraphDescription"
                                                     v-model="seoObjectSettingUpdateDto.OpenGraphDescription"
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
                                                  v-model="seoObjectSettingUpdateDto.TwitterTitle"
                                                  type="text"
                                                  placeholder="Twitter Başlığı" />
                                </b-form-group>
                                <b-form-group>
                                    <b-form-textarea id="TwitterDescription"
                                                     v-model="seoObjectSettingUpdateDto.TwitterDescription"
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
            <b-card v-show="postUpdateDto.PostType == '0'"
                    title="Ebeveyn Sayfa">
                <b-form-group>
                    <v-select v-model="selectedParentPage"
                              :options="parentPages"
                              label="Name"
                              :reduce="(option) => option.Id"
                              placeholder="Ebeveyn Sayfa Seçiniz..."
                              multiple
                              @input="changeParentPage" />
                </b-form-group>
            </b-card>
            <b-card v-show="postUpdateDto.PostType == '0'"
                    title="Alt Sayfa">
                <b-form-group>
                    <v-select v-model="selectedChildPage"
                              :options="childPages"
                              label="Name"
                              :reduce="(option) => option.Id"
                              placeholder="Alt Sayfa Seçiniz..."
                              multiple
                              @input="changeChildPage" />
                </b-form-group>
            </b-card>
            <b-card v-show="postUpdateDto.PostType == '1'"
                    title="Kategori">
                <b-form-group>
                    <v-select v-model="currentSelectedCategory"
                              :options="categories"
                              label="Name"
                              :reduce="(option) => option.Id"
                              placeholder="Kategori Seçiniz..."
                              multiple
                              @option:selecting="selectingTerms"
                              @option:deselected="deSelectingTerms" />
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
            <b-card v-show="postUpdateDto.PostType == '1'"
                    title="Etiket">
                <b-form-group>
                    <v-select v-model="currentSelectedTag"
                              :options="tags"
                              label="Name"
                              :reduce="(option) => option.Id"
                              placeholder="Etiket Seçiniz..."
                              multiple
                              @option:selecting="selectingTerms"
                              @option:deselecting="deSelectingTerms" />
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
                    <b-form-checkbox v-model="postUpdateDto.IsShowFeaturedImage"
                                     name="check-button"
                                     switch
                                     inline>
                        Görsel gönderide gösterilsin mi?
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
                <b-form-group v-show="postUpdateDto.PostType == '0'">
                    <b-form-checkbox v-model="postUpdateDto.IsShowSubPosts"
                                     name="check-button"
                                     switch
                                     inline>
                        Bu gönderi, diğer sayfalarda gösterilsin mi?
                    </b-form-checkbox>
                </b-form-group>
                <b-form-group v-show="postUpdateDto.PostType == '1'">
                    <b-form-checkbox v-model="postUpdateDto.CommentStatus"
                                     name="check-button"
                                     switch
                                     inline>
                        Bu gönderi için yorumlar aktif olsun mu?
                    </b-form-checkbox>
                </b-form-group>
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
    import ModalMedia from '../media/ModalMedia.vue';
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
                editorOption: {
                    placeholder: 'Makale İçeriği',
                    theme: 'snow'
                },
                required,
                isSpinnerShow: true,
                pageTitle: '',
                tooltipText: '',
                title: '',
                domainName: window.location.origin,
                isSlugEditActive: false,
                oldPostName: '',
                isParent: Boolean,
                postUpdateDto: {
                    Id: this.$route.query.edit,
                    Title: '',
                    PostName: '',
                    Content: '',
                    BottomContent: '',
                    PostType: '',
                    PostStatus: null,
                    CommentStatus: Boolean,
                    FeaturedImageId: '',
                    IsShowFeaturedImage: Boolean,
                    ParentId: '',
                    IsShowPage: Boolean,
                    IsShowSubPosts: Boolean,
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
                seoObjectSettingUpdateDto: {
                    Id: '',
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
                terms: [],
                categories: [],
                currentSelectedCategory: [],
                allParentTerms: [],
                selectedParentTerm: [],
                selectedParentTermValue: null,
                categoryName: '',
                tags: [],
                currentSelectedTag: [],
                tagName: '',
                selectedTerms: [],
                deSelectedTerms: [],
                parentPages: [],
                selectedParentPage: [],
                childPages: [],
                selectedChildPage: [],
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
                selectedPageType: '',
                selectedArticleType: ''
            }
        },
        methods: {
            postNameEdit() {
                this.isSlugEditActive = true;
                this.oldPostName = this.postUpdateDto.PostName;
            },
            postNameEditCancel() {
                this.isSlugEditActive = false;
                this.postUpdateDto.PostName = this.oldPostName;
            },
            selectingTerms(value) {
                var result = -1;
                this.terms.forEach((termId, index) => {
                    if (termId == value.Id) {
                        result = 1;
                    }
                    else {
                        result = 0;
                    }
                });
                if (result == 0) {
                    this.selectedTerms.push(value.Id);
                }
                console.log(result);
                console.log(this.selectedTerms);
            },
            deSelectingTerms(value) {
                this.terms.forEach((termId, index) => {
                    if (termId == value.Id) {
                        this.deSelectedTerms.push(termId);
                    }
                    else {
                        this.selectedTerms = this.selectedTerms.filter(element => element === value.Id);
                    }

                });
                console.log(this.selectedTerms);
                console.log(this.deSelectedTerms)
            },
            allCategories() {
                axios.get('/admin/term/allterms', {
                    params: {
                        term_type: 'category'
                    }
                })
                    .then((response) => {
                        if (response.data.ResultStatus === 0) {
                            this.categories = response.data.Terms;
                        }
                    })
                    .catch((error) => {
                        this.$toast({
                            component: ToastificationContent,
                            props: {
                                variant: 'danger',
                                title: 'Hata Oluştu!',
                                icon: 'AlertOctagonIcon',
                                text: 'Kategoriler listenirken hata oluştu. Lütfen tekrar deneyiniz.',
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
                                text: 'Etiketler listenirken hata oluştu. Lütfen tekrar deneyiniz.',
                            }
                        })
                    });
            },
            changeParentTerm(value) {
                this.categoryAddDto.ParentId = value;
            },
            changeParentPage() {

            },
            changeChildPage() {

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
                this.seoObjectSettingUpdateDto.SchemaPageType = value;
            },
            changeArticleType(value) {
                this.seoObjectSettingUpdateDto.SchemaArticleType = value;
            },
            getData() {
                axios.get('/admin/post/edit?post=' + this.$route.query.edit)
                    .then((response) => {
                        console.log(response.data);
                        if (response.data.PostUpdateDto != null) {
                            this.doHaveData = true;
                            this.postUpdateDto.PostType = response.data.PostUpdateDto.PostType;
                            this.postUpdateDto.Title = response.data.PostUpdateDto.Title;
                            this.postUpdateDto.PostName = response.data.PostUpdateDto.PostName;
                            this.postUpdateDto.Content = response.data.PostUpdateDto.Content;

                            this.postUpdateDto.IsShowFeaturedImage = response.data.PostUpdateDto.IsShowFeaturedImage;
                            if (response.data.PostUpdateDto.FeaturedImage != null) {
                                this.featuredImage.id = response.data.PostUpdateDto.FeaturedImageId;
                                this.featuredImage.fileName = response.data.PostUpdateDto.FeaturedImage.FileName;
                                this.featuredImage.altText = response.data.PostUpdateDto.FeaturedImage.AltText;
                            }

                            this.seoObjectSettingUpdateDto.Id = response.data.SeoObjectSettingUpdateDto.Id;
                            this.seoObjectSettingUpdateDto.SeoTitle = response.data.SeoObjectSettingUpdateDto.SeoTitle;
                            this.seoObjectSettingUpdateDto.SeoDescription = response.data.SeoObjectSettingUpdateDto.SeoDescription;
                            this.seoObjectSettingUpdateDto.CanonicalUrl = response.data.SeoObjectSettingUpdateDto.CanonicalUrl;
                            this.seoObjectSettingUpdateDto.IsRobotsNoIndex = response.data.SeoObjectSettingUpdateDto.IsRobotsNoIndex;
                            this.seoObjectSettingUpdateDto.IsRobotsNoFollow = response.data.SeoObjectSettingUpdateDto.IsRobotsNoFollow;
                            this.seoObjectSettingUpdateDto.IsRobotsNoArchive = response.data.SeoObjectSettingUpdateDto.IsRobotsNoArchive;
                            this.seoObjectSettingUpdateDto.IsRobotsNoImageIndex = response.data.SeoObjectSettingUpdateDto.IsRobotsNoImageIndex;
                            this.seoObjectSettingUpdateDto.IsRobotsNoSnippet = response.data.SeoObjectSettingUpdateDto.IsRobotsNoSnippet;

                            this.seoObjectSettingUpdateDto.SchemaPageType = response.data.SeoObjectSettingUpdateDto.SchemaPageType;
                            this.seoObjectSettingUpdateDto.SchemaArticleType = response.data.SeoObjectSettingUpdateDto.SchemaArticleType;

                            this.seoObjectSettingUpdateDto.OpenGraphTitle = response.data.SeoObjectSettingUpdateDto.OpenGraphTitle;
                            this.seoObjectSettingUpdateDto.OpenGraphDescription = response.data.SeoObjectSettingUpdateDto.OpenGraphDescription;

                            this.seoObjectSettingUpdateDto.TwitterTitle = response.data.SeoObjectSettingUpdateDto.TwitterTitle;
                            this.seoObjectSettingUpdateDto.TwitterDescription = response.data.SeoObjectSettingUpdateDto.TwitterDescription;

                            this.keywords = response.data.SeoObjectSettingUpdateDto.FocusKeyword == null ? [] : response.data.SeoObjectSettingUpdateDto.FocusKeyword.split(',');

                            if (response.data.SeoObjectSettingUpdateDto.OpenGraphImage != null) {
                                this.openGraphImage.id = response.data.SeoObjectSettingUpdateDto.OpenGraphImageId;
                                this.openGraphImage.fileName = response.data.SeoObjectSettingUpdateDto.OpenGraphImage.FileName;
                                this.openGraphImage.altText = response.data.SeoObjectSettingUpdateDto.OpenGraphImage.AltText;
                            }

                            if (response.data.SeoObjectSettingUpdateDto.TwitterImage != null) {
                                this.twitterImage.id = response.data.SeoObjectSettingUpdateDto.TwitterImageId;
                                this.twitterImage.fileName = response.data.SeoObjectSettingUpdateDto.TwitterImage.FileName;
                                this.twitterImage.altText = response.data.SeoObjectSettingUpdateDto.TwitterImage.AltText;
                            }

                            if (response.data.PostUpdateDto.PostType === 0) {
                                this.isParent = true;
                                this.postUpdateDto.ParentId = response.data.PostUpdateDto.ParentId;
                                if (response.data.PostUpdateDto.Parent != null) {
                                    this.selected = {
                                        Id: response.data.PostUpdateDto.Parent.Id,
                                        Name: response.data.PostUpdateDto.Parent.Name,
                                    }
                                }

                                this.pageTitle = "Sayfayı Düzenle";
                                this.tooltipText = "Yeni Sayfa Ekle";
                            } else if (response.data.PostUpdateDto.PostType === 1) {
                                this.pageTitle = "Makaleyi Düzenle";
                                this.tooltipText = "Yeni Makale Ekle";

                                this.postUpdateDto.CommentStatus = response.data.PostUpdateDto.CommentStatus;

                                if (response.data.PostUpdateDto.PostTerms.length > 0) {
                                    response.data.PostUpdateDto.PostTerms.forEach((term, index) => {
                                        if (term.TermType === 2) {
                                            this.currentSelectedCategory.push(term.TermId);
                                        } else if (term.TermType === 3) {
                                            this.currentSelectedTag.push(term.TermId);
                                        }
                                    });
                                    this.terms = this.currentSelectedCategory.concat(this.currentSelectedTag);
                                    console.log(this.terms);
                                }
                            }
                        }
                        else {
                            this.doHaveData = false;
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
                                text: 'Hata oluştu. Lütfen tekrar deneyin.',
                            }
                        })
                    });
            },
            validationForm: function (e) {
                this.postUpdateDto.FeaturedImageId = this.featuredImage.id;
                this.seoObjectSettingUpdateDto.FocusKeyword = this.keywords.toString();
                this.seoObjectSettingUpdateDto.OpenGraphImageId = this.openGraphImage.id;
                this.seoObjectSettingUpdateDto.TwitterImageId = this.twitterImage.id;

                if (e.target.id == "save") {
                    this.postUpdateDto.PostStatus = "publish";
                }
                else if (e.target.id == "draft") {
                    this.postUpdateDto.PostStatus = "draft";
                }
                this.$refs.articleAddForm.validate().then(success => {
                    if (success) {
                        axios.post('/admin/post/edit',
                            {
                                postUpdateDto: this.postUpdateDto,
                                SeoObjectSettingUpdateDto: this.seoObjectSettingUpdateDto
                            })
                            .then((response) => {

                                this.postTermAddDto.PostId = response.data.PostDto.Post.Id;

                                if (this.deSelectedTerms.length > 0) {
                                    console.log(this.deSelectedTerms);
                                    console.log(response.data.PostDto.Post.Id);

                                    this.deSelectedTerms.forEach((termId, index) => {
                                        console.log("termId: " + termId);
                                        axios.post('/admin/term/deletepostterm', {
                                            PostId: response.data.PostDto.Post.Id,
                                            TermId: termId,
                                        });
                                    });
                                }

                                if (this.selectedTerms.length > 0) {
                                    this.selectedTerms.forEach((postTerm, index) => {
                                        this.postTermAddDto.TermId = postTerm.Id;
                                        this.postTermAddDto.TermType = postTerm.TermType;

                                        axios.post('/admin/term/newpostterm', {
                                            PostTermAddDto: this.postTermAddDto
                                        }).catch((error) => {
                                            console.log("newpost");
                                            console.log(error);
                                            console.log(error.request);
                                        });
                                    });
                                }

                                //if (this.selectedTag.length > 0) {
                                //    for (var i = 0; i < this.selectedTag.length; i++) {
                                //        this.postTermAddDto.TermId = this.selectedTag[i];
                                //        this.postTermAddDto.TermType = "tag";

                                //        axios.post('/admin/term/newpostterm', {
                                //            PostTermAddDto: this.postTermAddDto
                                //        });
                                //    }
                                //}

                                if (response.data.PostDto.ResultStatus === 0) {
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
                                    this.currentSelectedCategory.push(response.data.TermDto.Term.Id)
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
                                    this.currentSelectedTag.push(response.data.TermDto.Term.Id)
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
            this.getData();
            this.getSchnemaPageType();
            this.getSchnemaArticleType();
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