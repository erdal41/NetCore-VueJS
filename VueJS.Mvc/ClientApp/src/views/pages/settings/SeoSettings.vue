<template>
    <b-row>
        <b-col cols="12">
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
                    <b-button v-if="$can('update', 'Seo')"
                              variant="primary"
                              type="button"
                              :disabled="overlayShow"
                              @click.prevent="updateData">
                        Güncelle
                    </b-button>
                </b-col>
            </b-row>
            <b-overlay :show="overlayShow"
                       rounded="sm"
                       opacity="0.80"
                       variant="white"
                       no-fade>
                <b-row>
                    <b-col cols="12">
                        <b-tabs id="tab-seo-settings"
                                vertical
                                content-class="col-12 col-md-9 mt-1 mt-md-0"
                                pills
                                nav-wrapper-class="col-md-3 col-12"
                                nav-class="nav-left">
                            <b-tab active>
                                <template #title>
                                    <feather-icon icon="HexagonIcon"
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
                                                              v-model="seoGeneralSettingUpdateDto.GoogleVerificationCode"
                                                              placeholder="Google doğrulama kodunu giriniz." />
                                            </b-form-group>
                                            <b-form-group label="Yandex Doğrulama Kodu"
                                                          label-for="yandexVerificationCode">
                                                <b-form-input id="yandexVerificationCode"
                                                              v-model="seoGeneralSettingUpdateDto.YandexVerificationCode"
                                                              placeholder="Yandex doğrulama kodunu giriniz." />
                                            </b-form-group>
                                            <b-form-group label="Bing Doğrulama Kodu"
                                                          label-for="bingVerificationCode">
                                                <b-form-input id="bingVerificationCode"
                                                              v-model="seoGeneralSettingUpdateDto.BingVerificationCode"
                                                              placeholder="Bing doğrulama kodunu giriniz." />
                                            </b-form-group>
                                            <b-form-group label="Baidu Doğrulama Kodu"
                                                          label-for="baiduVerificationCode">
                                                <b-form-input id="baiduVerificationCode"
                                                              v-model="seoGeneralSettingUpdateDto.BaiduVerificationCode"
                                                              placeholder="Bauidu doğrulama kodunu giriniz." />
                                            </b-form-group>
                                        </b-card>
                                    </b-tab>
                                    <b-tab title="Özellikler">
                                        <b-card>
                                            <b-form-group>
                                                <b-form-checkbox v-model="seoGeneralSettingUpdateDto.IsActiveSitemap"
                                                                 name="check-button"
                                                                 switch
                                                                 inline>
                                                    XML site haritaları aktif olsun mu?
                                                </b-form-checkbox>
                                                <a href="#">Görüntüle</a>
                                            </b-form-group>
                                            <b-form-group>
                                                <b-form-checkbox v-model="seoGeneralSettingUpdateDto.IsActiveRobotsTxt"
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
                                        <b-card>
                                            <h5>Temel Ayarlar</h5>
                                            <small>Google'ın web site içeriğinizi daha iyi anlaması için aşağıdan ilgili seçiminizi yapmanızı önermekteyiz.</small>
                                            <b-form-group label="Sitenin bir firmayı mı yoksa bir kişiyi mi temsil ettiğini seçiniz."
                                                          label-for="seoType"
                                                          class="mt-2">
                                                <v-select id="seoType"
                                                          v-model="seoGeneralSettingUpdateDto.SeoType"
                                                          :value="seoGeneralSettingUpdateDto.SeoType"
                                                          :options="seoTypes"
                                                          label="Name"
                                                          :reduce="(option) => option.Id"
                                                          :clearable="false"
                                                          placeholder="— Seçim Yapın —" />
                                            </b-form-group>
                                            <b-form-group v-if="seoGeneralSettingUpdateDto.SeoType === 0"
                                                          label="Firma Adı"
                                                          label-for="siteName">
                                                <b-form-input id="siteName"
                                                              v-model="seoGeneralSettingUpdateDto.SiteName"
                                                              placeholder="Firma adınızı giriniz." />
                                            </b-form-group>

                                            <h5 class="mt-3">Varsayılan Resim</h5>
                                            <small> Öne çıkan görsel koymadığınız tüm paylaşımlarda bu resim kullanılır.</small>
                                            <div class="image-preview mt-2">
                                                <div class="image-thumbnail select-sitemain-image"
                                                     v-b-modal.modal-media
                                                     @click="selectImage">
                                                    <b-img rounded
                                                           v-bind:src="siteMainImage.fileName == null ? noImage : require('@/assets/images/media/' + siteMainImage.fileName)"
                                                           :alt="siteMainImage.altText"
                                                           class="select-sitemain-image" />
                                                    <b-button v-ripple.400="'rgba(186, 191, 199, 0.15)'"
                                                              variant="relief-primary"
                                                              size="sm"
                                                              class="btn-icon rounded-circle select-image select-sitemain-image">
                                                        <feather-icon icon="Edit2Icon"
                                                                      class="select-sitemain-image"
                                                                      size="11" />
                                                    </b-button>
                                                </div>
                                                <b-button v-ripple.400="'rgba(186, 191, 199, 0.15)'"
                                                          variant="relief-secondary"
                                                          size="sm"
                                                          class="btn-icon rounded-circle remove-image remove-sitemain-image"
                                                          @click="removeImage">
                                                    <feather-icon icon="XIcon"
                                                                  class="remove-sitemain-image"
                                                                  size="11" />
                                                </b-button>
                                                <b-form-input type="text"
                                                              hidden
                                                              v-model="siteMainImage.id"></b-form-input>
                                            </div>
                                        </b-card>
                                    </b-tab>
                                    <b-tab title="Sayfa - Makale">
                                        <b-card-actions id="card-page"
                                                        title="Sayfalar"
                                                        action-collapse
                                                        collapsed>
                                            <b-form-group>
                                                <b-form-checkbox v-model="seoGeneralSettingUpdateDto.IsActivePageSearchEngineIndex"
                                                                 name="check-button"
                                                                 switch
                                                                 inline>
                                                    Sayfalar arama sonuçlarında gösterilsin mi?
                                                </b-form-checkbox>
                                            </b-form-group>
                                            <b-form-group>
                                                <b-form-checkbox v-model="seoGeneralSettingUpdateDto.IsActivePageSeoSetting"
                                                                 name="check-button"
                                                                 switch
                                                                 inline>
                                                    Sayfalar için SEO ayarları gösterilsin mi?
                                                </b-form-checkbox>
                                            </b-form-group>
                                            <b-form-group label="Varsayılan SEO Başlığı"
                                                          label-for="pageDefaultSeoTitle">
                                                <b-form-input id="pageDefaultSeoTitle"
                                                              v-model="seoGeneralSettingUpdateDto.PageDefaultSeoTitle"
                                                              placeholder="SEO başlığını giriniz." />
                                            </b-form-group>
                                            <b-form-group label="Varsayılan Meta Açıklama"
                                                          label-for="pageDefaultSeoDescription">
                                                <b-form-textarea id="pageDefaultSeoDescription"
                                                                 v-model="seoGeneralSettingUpdateDto.PageDefaultSeoDescription"
                                                                 placeholder="Meta açıklamanızı giriniz."
                                                                 rows="2" />
                                            </b-form-group>
                                            <b-form-group label="Sayfa Türü"
                                                          label-for="pageSchemaPageType">
                                                <v-select id="pageSchemaPageType"
                                                          v-model="seoGeneralSettingUpdateDto.PageSchemaPageType"
                                                          :value="seoGeneralSettingUpdateDto.PageSchemaPageType"
                                                          :options="pageSchemaPageTypes"
                                                          label="Name"
                                                          :reduce="(option) => option.Id"
                                                          :clearable="false" />
                                            </b-form-group>
                                            <b-form-group label="Makale Türü"
                                                          label-for="pageSchemaArticleType">
                                                <v-select id="pageSchemaArticleType"
                                                          v-model="seoGeneralSettingUpdateDto.PageSchemaArticleType"
                                                          :value="seoGeneralSettingUpdateDto.PageSchemaArticleType"
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
                                                              v-model="seoGeneralSettingUpdateDto.PageSocialTitle"
                                                              type="text"
                                                              placeholder="Sosyal medya başlığını giriniz." />
                                            </b-form-group>
                                            <b-form-group label="Sosyal Medya Paylaşım Açıklaması"
                                                          label-for="pageSocialDescription">
                                                <b-form-textarea id="pageSocialDescription"
                                                                 v-model="seoGeneralSettingUpdateDto.PageSocialDescription"
                                                                 placeholder="Sosyal medya açıklamasını giriniz."
                                                                 rows="2" />
                                            </b-form-group>
                                        </b-card-actions>
                                        <b-card-actions id="card-article"
                                                        title="Makaleler"
                                                        action-collapse
                                                        collapsed>
                                            <b-form-group>
                                                <b-form-checkbox v-model="seoGeneralSettingUpdateDto.IsActiveArticleSearchEngineIndex"
                                                                 name="check-button"
                                                                 switch
                                                                 inline>
                                                    Makaleler arama sonuçlarında gösterilsin mi?
                                                </b-form-checkbox>
                                            </b-form-group>
                                            <b-form-group>
                                                <b-form-checkbox v-model="seoGeneralSettingUpdateDto.IsActiveArticleSeoSetting"
                                                                 name="check-button"
                                                                 switch
                                                                 inline>
                                                    Makaleler için SEO ayarları gösterilsin mi?
                                                </b-form-checkbox>
                                            </b-form-group>
                                            <b-form-group label="Varsayılan SEO Başlığı"
                                                          label-for="articleDefaultSeoTitle">
                                                <b-form-input id="articleDefaultSeoTitle"
                                                              v-model="seoGeneralSettingUpdateDto.ArticleDefaultSeoTitle"
                                                              placeholder="SEO başlığını giriniz." />
                                            </b-form-group>
                                            <b-form-group label="Varsayılan Meta Açıklama"
                                                          label-for="articleDefaultSeoDescription">
                                                <b-form-textarea id="articleDefaultSeoDescription"
                                                                 v-model="seoGeneralSettingUpdateDto.ArticleDefaultSeoDescription"
                                                                 placeholder="Meta açıklamanızı giriniz."
                                                                 rows="2" />
                                            </b-form-group>
                                            <b-form-group label="Sayfa Türü"
                                                          label-for="articleSchemaPageType">
                                                <v-select id="articleSchemaPageType"
                                                          v-model="seoGeneralSettingUpdateDto.ArticleSchemaPageType"
                                                          :value="seoGeneralSettingUpdateDto.ArticleSchemaPageType"
                                                          :options="articleSchemaPageTypes"
                                                          label="Name"
                                                          :reduce="(option) => option.Id"
                                                          :clearable="false" />
                                            </b-form-group>
                                            <b-form-group label="Makale Türü"
                                                          label-for="articleSchemaArticleType">
                                                <v-select id="articleSchemaArticleType"
                                                          v-model="seoGeneralSettingUpdateDto.ArticleSchemaArticleType"
                                                          :value="seoGeneralSettingUpdateDto.ArticleSchemaArticleType"
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
                                                              v-model="seoGeneralSettingUpdateDto.ArticleSocialTitle"
                                                              type="text"
                                                              placeholder="Sosyal medya başlığını giriniz." />
                                            </b-form-group>
                                            <b-form-group label="Sosyal Medya Paylaşım Açıklaması"
                                                          label-for="articleSocialDescription">
                                                <b-form-textarea id="articleSocialDescription"
                                                                 v-model="seoGeneralSettingUpdateDto.ArticleSocialDescription"
                                                                 placeholder="Sosyal medya açıklamasını giriniz."
                                                                 rows="2" />
                                            </b-form-group>
                                        </b-card-actions>
                                    </b-tab>
                                    <b-tab title="Kategori - Etiket">
                                        <b-card-actions id="card-category"
                                                        title="Kategoriler"
                                                        action-collapse
                                                        collapsed>
                                            <b-form-group>
                                                <b-form-checkbox v-model="seoGeneralSettingUpdateDto.IsActiveCategorySearchEngineIndex"
                                                                 name="check-button"
                                                                 switch
                                                                 inline>
                                                    Kategoriler arama sonuçlarında gösterilsin mi?
                                                </b-form-checkbox>
                                            </b-form-group>
                                            <b-form-group>
                                                <b-form-checkbox v-model="seoGeneralSettingUpdateDto.IsActiveCategorySeoSetting"
                                                                 name="check-button"
                                                                 switch
                                                                 inline>
                                                    Kategoriler için SEO ayarları gösterilsin mi?
                                                </b-form-checkbox>
                                            </b-form-group>
                                            <b-form-group label="Varsayılan SEO Başlığı"
                                                          label-for="categoryDefaultSeoTitle">
                                                <b-form-input id="categoryDefaultSeoTitle"
                                                              v-model="seoGeneralSettingUpdateDto.CategoryDefaultSeoTitle"
                                                              placeholder="SEO başlığını giriniz." />
                                            </b-form-group>
                                            <b-form-group label="Varsayılan Meta Açıklama"
                                                          label-for="categoryDefaultSeoDescription">
                                                <b-form-textarea id="categoryDefaultSeoDescription"
                                                                 v-model="seoGeneralSettingUpdateDto.CategoryDefaultSeoDescription"
                                                                 placeholder="Meta açıklamanızı giriniz."
                                                                 rows="2" />
                                            </b-form-group>

                                            <h5 class="mt-3 mb-2">Kategoriler İçin Varsayılan Sosyal Medya Ayarları</h5>
                                            <div class="image-preview">
                                                <div class="image-thumbnail select-categorysocial-image"
                                                     v-b-modal.modal-media
                                                     @click="selectImage">
                                                    <b-img rounded
                                                           v-bind:src="categorySocialImage.fileName == null ? noImage : require('@/assets/images/media/' + categorySocialImage.fileName)"
                                                           :alt="categorySocialImage.altText"
                                                           class="select-categorysocial-image" />
                                                    <b-button v-ripple.400="'rgba(186, 191, 199, 0.15)'"
                                                              variant="relief-primary"
                                                              size="sm"
                                                              class="btn-icon rounded-circle select-image select-categorysocial-image">
                                                        <feather-icon icon="Edit2Icon"
                                                                      class="select-categorysocial-image"
                                                                      size="11" />
                                                    </b-button>
                                                </div>
                                                <b-button v-ripple.400="'rgba(186, 191, 199, 0.15)'"
                                                          variant="relief-secondary"
                                                          size="sm"
                                                          class="btn-icon rounded-circle remove-image remove-categorysocial-image"
                                                          @click="removeImage">
                                                    <feather-icon icon="XIcon"
                                                                  class="remove-categorysocial-image"
                                                                  size="11" />
                                                </b-button>
                                                <b-form-input type="text"
                                                              hidden
                                                              v-model="categorySocialImage.id"></b-form-input>
                                            </div>
                                            <b-form-group label="Sosyal Medya Paylaşım Başlığı"
                                                          label-for="categorySocialTitle"
                                                          class="mt-1">
                                                <b-form-input id="categorySocialTitle"
                                                              v-model="seoGeneralSettingUpdateDto.CategorySocialTitle"
                                                              type="text"
                                                              placeholder="Sosyal medya başlığını giriniz." />
                                            </b-form-group>
                                            <b-form-group label="Sosyal Medya Paylaşım Açıklaması"
                                                          label-for="categorySocialDescription">
                                                <b-form-textarea id="categorySocialDescription"
                                                                 v-model="seoGeneralSettingUpdateDto.CategorySocialDescription"
                                                                 placeholder="Sosyal medya açıklamasını giriniz."
                                                                 rows="2" />
                                            </b-form-group>
                                        </b-card-actions>
                                        <b-card-actions id="card-tag"
                                                        title="Etiketler"
                                                        action-collapse
                                                        collapsed>
                                            <b-form-group>
                                                <b-form-checkbox v-model="seoGeneralSettingUpdateDto.IsActiveTagSearchEngineIndex"
                                                                 name="check-button"
                                                                 switch
                                                                 inline>
                                                    Etiketler arama sonuçlarında gösterilsin mi?
                                                </b-form-checkbox>
                                            </b-form-group>
                                            <b-form-group>
                                                <b-form-checkbox v-model="seoGeneralSettingUpdateDto.IsActiveTagSeoSetting"
                                                                 name="check-button"
                                                                 switch
                                                                 inline>
                                                    Etiketler için SEO ayarları gösterilsin mi?
                                                </b-form-checkbox>
                                            </b-form-group>
                                            <b-form-group label="Varsayılan SEO Başlığı"
                                                          label-for="tagDefaultSeoTitle">
                                                <b-form-input id="tagDefaultSeoTitle"
                                                              v-model="seoGeneralSettingUpdateDto.TagDefaultSeoTitle"
                                                              placeholder="SEO başlığını giriniz." />
                                            </b-form-group>
                                            <b-form-group label="Varsayılan Meta Açıklama"
                                                          label-for="tagDefaultSeoDescription">
                                                <b-form-textarea id="tagDefaultSeoDescription"
                                                                 v-model="seoGeneralSettingUpdateDto.TagDefaultSeoDescription"
                                                                 placeholder="Meta açıklamanızı giriniz."
                                                                 rows="2" />
                                            </b-form-group>

                                            <h5 class="mt-3 mb-2">Etiketler İçin Varsayılan Sosyal Medya Ayarları</h5>
                                            <div class="image-preview">
                                                <div class="image-thumbnail select-tagsocial-image"
                                                     v-b-modal.modal-media
                                                     @click="selectImage">
                                                    <b-img rounded
                                                           v-bind:src="tagSocialImage.fileName == null ? noImage : require('@/assets/images/media/' + tagSocialImage.fileName)"
                                                           :alt="tagSocialImage.altText"
                                                           class="select-tagsocial-image" />
                                                    <b-button v-ripple.400="'rgba(186, 191, 199, 0.15)'"
                                                              variant="relief-primary"
                                                              size="sm"
                                                              class="btn-icon rounded-circle select-image select-tagsocial-image">
                                                        <feather-icon icon="Edit2Icon"
                                                                      class="select-tagsocial-image"
                                                                      size="11" />
                                                    </b-button>
                                                </div>
                                                <b-button v-ripple.400="'rgba(186, 191, 199, 0.15)'"
                                                          variant="relief-secondary"
                                                          size="sm"
                                                          class="btn-icon rounded-circle remove-image remove-tagsocial-image"
                                                          @click="removeImage">
                                                    <feather-icon icon="XIcon"
                                                                  class="remove-tagsocial-image"
                                                                  size="11" />
                                                </b-button>
                                                <b-form-input type="text"
                                                              hidden
                                                              v-model="tagSocialImage.id"></b-form-input>
                                            </div>
                                            <b-form-group label="Sosyal Medya Paylaşım Başlığı"
                                                          label-for="tagSocialTitle"
                                                          class="mt-1">
                                                <b-form-input id="tagSocialTitle"
                                                              v-model="seoGeneralSettingUpdateDto.TagSocialTitle"
                                                              type="text"
                                                              placeholder="Sosyal medya başlığını giriniz." />
                                            </b-form-group>
                                            <b-form-group label="Sosyal Medya Paylaşım Açıklaması"
                                                          label-for="tagSocialDescription">
                                                <b-form-textarea id="tagSocialDescription"
                                                                 v-model="seoGeneralSettingUpdateDto.TagSocialDescription"
                                                                 placeholder="Sosyal medya açıklamasını giriniz."
                                                                 rows="2" />
                                            </b-form-group>
                                        </b-card-actions>
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
                                        <b-card>
                                            <small>Arama motorlarının hangi sosyal profillerin bu site ile bağlantılı olduğunu anlayabilmeleri için, sitenizin sosyal profillerini aşağıdaki alana giriniz.</small>
                                            <b-form-group label="Facebook Sayfa Linki"
                                                          label-for="facebookPageLink"
                                                          class="mt-1">
                                                <b-form-input id="facebookPageLink"
                                                              v-model="seoGeneralSettingUpdateDto.FacebookPageLink"
                                                              type="text"
                                                              placeholder="Facebook sayfa linkini giriniz." />
                                            </b-form-group>
                                            <b-form-group label="Twitter Kullanıcı Adı"
                                                          label-for="twitterUserName">
                                                <b-form-input id="twitterUserName"
                                                              v-model="seoGeneralSettingUpdateDto.TwitterUserName"
                                                              type="text"
                                                              placeholder="Twitter kullanıcı adını giriniz." />
                                            </b-form-group>
                                            <b-form-group label="Instagram Adresi"
                                                          label-for="instagramProfilLink">
                                                <b-form-input id="instagramProfilLink"
                                                              v-model="seoGeneralSettingUpdateDto.InstagramProfilLink"
                                                              type="text"
                                                              placeholder="Instagram adresini giriniz." />
                                            </b-form-group>
                                            <b-form-group label="LinkedIn Adresi"
                                                          label-for="linkedInProfilLink">
                                                <b-form-input id="linkedInProfilLink"
                                                              v-model="seoGeneralSettingUpdateDto.LinkedInProfilLink"
                                                              type="text"
                                                              placeholder="LinkedIn adresini giriniz." />
                                            </b-form-group>
                                            <b-form-group label="Pinterest Adresi"
                                                          label-for="pinterestProfilLink">
                                                <b-form-input id="pinterestProfilLink"
                                                              v-model="seoGeneralSettingUpdateDto.PinterestProfilLink"
                                                              type="text"
                                                              placeholder="Pinterest adresini giriniz." />
                                            </b-form-group>
                                            <b-form-group label="YouTube Adresi"
                                                          label-for="youtubeChannelLink">
                                                <b-form-input id="youtubeChannelLink"
                                                              v-model="seoGeneralSettingUpdateDto.YoutubeChannelLink"
                                                              type="text"
                                                              placeholder="YouTube adresini giriniz." />
                                            </b-form-group>
                                            <b-form-group label="Wikipedia Adresi"
                                                          label-for="wikipediaLink">
                                                <b-form-input id="wikipediaLink"
                                                              v-model="seoGeneralSettingUpdateDto.WikipediaLink"
                                                              type="text"
                                                              placeholder="Wikipedia adresini giriniz." />
                                            </b-form-group>
                                        </b-card>
                                    </b-tab>
                                    <b-tab title="Sosyal Medya">
                                        <b-card>
                                            <small>Sitenizden bir bağlantı paylaşıldığında Facebook'un ve diğer sosyal medya ortamlarında ön izleme görseli ve metin alıntısı eklemesini istiyorsanız bu özelliği etkinleştirin.</small>
                                            <b-form-group class="mt-1">
                                                <b-form-checkbox v-model="seoGeneralSettingUpdateDto.IsActiveOpenGraph"
                                                                 name="check-button"
                                                                 switch
                                                                 inline>
                                                    Sosyal medya meta verileri aktif olsun mu?
                                                </b-form-checkbox>
                                            </b-form-group>

                                            <h5 class="mt-3">Varsayılan Resim</h5>
                                            <small>Paylaşıma sunulan her bir gönderi herhangi bir görüntü içermiyorsa bu görsel kullanılır.</small>
                                            <div class="image-preview mt-2">
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
                                        </b-card>
                                    </b-tab>
                                    <b-tab title="Twitter">
                                        <b-card>
                                            <small>
                                                Twitter, Facebook gibi Open Graph meta verilerini kullanır; bu nedenle, sitenizi Twitter için optimize etmek istiyorsanız,
                                                Sosyal Medya sekmesindeki "Sosyal medya meta verileri" ayarını etkinleştirdiğinizden emin olun.
                                            </small>
                                            <b-form-group class="mt-1">
                                                <b-form-checkbox v-model="seoGeneralSettingUpdateDto.IsActiveTwitterOpenGraph"
                                                                 name="check-button"
                                                                 switch
                                                                 inline>
                                                    Twitter meta verisi aktif olsun mu?
                                                </b-form-checkbox>
                                            </b-form-group>
                                            <h5 class="mt-3">Varsayılan Ayar</h5>
                                            <b-form-group label="Sitenizden bir bağlantı paylaşıldığında Twitter'ın ön izleme görseli ve metin alıntısı eklemesini istiyorsanız bu özelliği etkinleştirin"
                                                          label-for="twitterCardType">
                                                <v-select id="twitterCardType"
                                                          v-model="seoGeneralSettingUpdateDto.TwitterCardType"
                                                          :value="seoGeneralSettingUpdateDto.TwitterCardType"
                                                          :options="twitterCardTypes"
                                                          label="Name"
                                                          :reduce="(option) => option.Id"
                                                          :clearable="false" />
                                            </b-form-group>
                                        </b-card>
                                    </b-tab>
                                    <b-tab title="Pinterest">
                                        <b-card>
                                            <small>
                                                Pinterest, Facebook gibi Open Graph meta verilerini kullanır; bu nedenle, sitenizi Pinterest için optimize etmek istiyorsanız,
                                                Sosyal Medya sekmesindeki "Sosyal medya meta verileri" ayarını etkinleştirdiğinizden emin olun.
                                            </small>
                                            <b-form-group label="Pinterest Onay Kodu"
                                                          label-for="pinterestConfirmationCode"
                                                          class="mt-1">
                                                <b-form-input id="pinterestConfirmationCode"
                                                              v-model="seoGeneralSettingUpdateDto.PinterestConfirmationCode"
                                                              type="text"
                                                              placeholder="Pinterest onay kodunuzu giriniz." />
                                            </b-form-group>
                                        </b-card>
                                    </b-tab>
                                </b-tabs>
                            </b-tab>
                        </b-tabs>
                    </b-col>
                </b-row>
            </b-overlay>
        </b-col>
    </b-row>
</template>

<script>
    import ModalMedia from '../media/ModalMedia.vue';
    import BCardActions from '@core/components/b-card-actions/BCardActions.vue';
    import vSelect from 'vue-select';
    import { BRow, BCol, BBreadcrumb, BBreadcrumbItem, BOverlay, BButton, BTabs, BTab, BCard, BSpinner, BImg, BFormGroup, BFormInput, BFormCheckbox, BFormTextarea } from 'bootstrap-vue';
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
            BOverlay,
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
                overlayShow: true,
                noImage: require('@/assets/images/default/default-post-image.jpg'),
                isSiteMainImageChoose: false,
                isPageSocialImageChoose: false,
                isArticleSocialImageChoose: false,
                isCategorySocialImageChoose: false,
                isTagSocialImageChoose: false,
                isOpenGraphImageChoose: false,
                siteMainImage: {
                    id: null,
                    fileName: null,
                    altText: null,
                },
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
                openGraphImage: {
                    id: null,
                    fileName: null,
                    altText: null,
                },
                seoGeneralSettingUpdateDto: {
                    Id: 1,
                    GoogleVerificationCode: '',
                    YandexVerificationCode: '',
                    BingVerificationCode: '',
                    BaiduVerificationCode: '',
                    IsActiveSitemap: false,
                    IsActiveRobotsTxt: false,
                    SeoType: '',
                    SiteName: '',
                    SiteMainImageId: '',
                    IsActivePageSearchEngineIndex: false,
                    IsActivePageSeoSetting: false,
                    PageDefaultSeoTitle: '',
                    PageDefaultSeoDescription: '',
                    PageSchemaPageType: '',
                    PageSchemaArticleType: '',
                    PageSocialTitle: '',
                    PageSocialDescription: '',
                    PageSocialImageId: '',
                    IsActiveArticleSearchEngineIndex: false,
                    IsActiveArticleSeoSetting: false,
                    ArticleDefaultSeoTitle: '',
                    ArticleDefaultSeoDescription: '',
                    ArticleSchemaPageType: '',
                    ArticleSchemaArticleType: '',
                    ArticleSocialImageId: '',
                    ArticleSocialTitle: '',
                    ArticleSocialDescription: '',
                    IsActiveCategorySearchEngineIndex: false,
                    IsActiveCategorySeoSetting: false,
                    CategoryDefaultSeoTitle: '',
                    CategoryDefaultSeoDescription: '',
                    CategorySocialImageId: '',
                    CategorySocialTitle: '',
                    CategorySocialDescription: '',
                    IsActiveTagSearchEngineIndex: false,
                    IsActiveTagSeoSetting: false,
                    TagDefaultSeoTitle: '',
                    TagDefaultSeoDescription: '',
                    TagSocialImageId: '',
                    TagSocialTitle: '',
                    TagSocialDescription: '',
                    FacebookPageLink: '',
                    TwitterUserName: '',
                    InstagramProfilLink: '',
                    LinkedInProfilLink: '',
                    PinterestProfilLink: '',
                    YoutubeChannelLink: '',
                    WikipediaLink: '',
                    IsActiveOpenGraph: false,
                    OpenGraphImageId: '',
                    IsActiveTwitterOpenGraph: false,
                    TwitterCardType: '',
                    PinterestConfirmationCode: '',
                },
                seoTypes: [],
                pageSchemaPageTypes: [],
                pageSchemaArticleTypes: [],
                articleSchemaPageTypes: [],
                articleSchemaArticleTypes: [],
                twitterCardTypes: [],
            }
        },
        methods: {
            imageChange(id, name, altText) {
                if (this.isSiteMainImageChoose == true) {
                    this.siteMainImage.id = id;
                    this.siteMainImage.fileName = name;
                    this.siteMainImage.altText = altText;
                } else if (this.isPageSocialImageChoose == true) {
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
                } else if (this.isOpenGraphImageChoose == true) {
                    this.openGraphImage.id = id;
                    this.openGraphImage.fileName = name;
                    this.openGraphImage.altText = altText;
                }

                this.isSiteMainImageChoose = false;
                this.isPageSocialImageChoose = false;
                this.isArticleSocialImageChoose = false;
                this.isCategorySocialImageChoose = false;
                this.isTagSocialImageChoose = false;
                this.isOpenGraphImageChoose = false;
            },
            selectImage(e) {
                if (e.target._prevClass.includes('select-sitemain-image')) {
                    this.isSiteMainImageChoose = true;
                } else if (e.target._prevClass.includes('select-pagesocial-image')) {
                    this.isPageSocialImageChoose = true;
                } else if (e.target._prevClass.includes('select-articlesocial-image')) {
                    this.isArticleSocialImageChoose = true;
                } else if (e.target._prevClass.includes('select-categorysocial-image')) {
                    this.isCategorySocialImageChoose = true;
                } else if (e.target._prevClass.includes('select-tagsocial-image')) {
                    this.isTagSocialImageChoose = true;
                } else if (e.target._prevClass.includes('select-opengraph-image')) {
                    this.isOpenGraphImageChoose = true;
                }
            },
            removeImage: function (e) {
                if (e.target._prevClass.includes('remove-sitemain-image')) {
                    this.siteMainImage.id = null;
                    this.siteMainImage.fileName = null;
                    this.siteMainImage.altText = null;
                } else if (e.target._prevClass.includes('remove-pagesocial-image')) {
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
                } else if (e.target._prevClass.includes('remove-tagsocial-image')) {
                    this.tagSocialImage.id = null;
                    this.tagSocialImage.fileName = null;
                    this.tagSocialImage.altText = null;
                } else if (e.target._prevClass.includes('remove-opengraph-image')) {
                    this.openGraphImage.id = null;
                    this.openGraphImage.fileName = null;
                    this.openGraphImage.altText = null;
                }
            },
            getSeoType() {
                axios.get('/admin/settings-getseotype')
                    .then((response) => {
                        this.seoTypes = response.data;
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
            getSchemaPageType() {
                axios.get('/admin/settings-getschemapagetype')
                    .then((response) => {
                        this.pageSchemaPageTypes = response.data;
                        this.articleSchemaPageTypes = response.data;
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
                axios.get('/admin/settings-getschemaarticletype')
                    .then((response) => {
                        this.pageSchemaArticleTypes = response.data;
                        this.articleSchemaArticleTypes = response.data;
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
            getTwitterCardType() {
                axios.get('/admin/settings-gettwittercardtype')
                    .then((response) => {
                        this.twitterCardTypes = response.data;
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
            getAllData() {
                axios.get('/admin/settings-seogeneral')
                    .then((response) => {
                        console.log(response.data)
                        if (response.data.SeoGeneralSettingUpdateDto.ResultStatus === 0) {
                            this.seoGeneralSettingUpdateDto.GoogleVerificationCode = response.data.SeoGeneralSettingUpdateDto.Data.GoogleVerificationCode;
                            this.seoGeneralSettingUpdateDto.YandexVerificationCode = response.data.SeoGeneralSettingUpdateDto.Data.YandexVerificationCode;
                            this.seoGeneralSettingUpdateDto.BingVerificationCode = response.data.SeoGeneralSettingUpdateDto.Data.BingVerificationCode;
                            this.seoGeneralSettingUpdateDto.BaiduVerificationCode = response.data.SeoGeneralSettingUpdateDto.Data.BaiduVerificationCode;
                            this.seoGeneralSettingUpdateDto.IsActiveSitemap = response.data.SeoGeneralSettingUpdateDto.Data.IsActiveSitemap;
                            this.seoGeneralSettingUpdateDto.IsActiveRobotsTxt = response.data.SeoGeneralSettingUpdateDto.Data.IsActiveRobotsTxt;
                            this.seoGeneralSettingUpdateDto.SeoType = response.data.SeoGeneralSettingUpdateDto.Data.SeoType;
                            this.seoGeneralSettingUpdateDto.SiteName = response.data.SeoGeneralSettingUpdateDto.Data.SiteName;
                            this.seoGeneralSettingUpdateDto.IsActivePageSearchEngineIndex = response.data.SeoGeneralSettingUpdateDto.Data.IsActivePageSearchEngineIndex;
                            this.seoGeneralSettingUpdateDto.IsActivePageSeoSetting = response.data.SeoGeneralSettingUpdateDto.Data.IsActivePageSeoSetting;
                            this.seoGeneralSettingUpdateDto.PageDefaultSeoTitle = response.data.SeoGeneralSettingUpdateDto.Data.PageDefaultSeoTitle;
                            this.seoGeneralSettingUpdateDto.PageDefaultSeoDescription = response.data.SeoGeneralSettingUpdateDto.Data.PageDefaultSeoDescription;
                            this.seoGeneralSettingUpdateDto.PageSchemaPageType = response.data.SeoGeneralSettingUpdateDto.Data.PageSchemaPageType;
                            this.seoGeneralSettingUpdateDto.PageSchemaArticleType = response.data.SeoGeneralSettingUpdateDto.Data.PageSchemaArticleType;
                            this.seoGeneralSettingUpdateDto.PageSocialTitle = response.data.SeoGeneralSettingUpdateDto.Data.PageSocialTitle;
                            this.seoGeneralSettingUpdateDto.PageSocialDescription = response.data.SeoGeneralSettingUpdateDto.Data.PageSocialDescription;
                            this.seoGeneralSettingUpdateDto.IsActiveArticleSearchEngineIndex = response.data.SeoGeneralSettingUpdateDto.Data.IsActiveArticleSearchEngineIndex;
                            this.seoGeneralSettingUpdateDto.IsActiveArticleSeoSetting = response.data.SeoGeneralSettingUpdateDto.Data.IsActiveArticleSeoSetting;
                            this.seoGeneralSettingUpdateDto.ArticleDefaultSeoTitle = response.data.SeoGeneralSettingUpdateDto.Data.ArticleDefaultSeoTitle;
                            this.seoGeneralSettingUpdateDto.ArticleDefaultSeoDescription = response.data.SeoGeneralSettingUpdateDto.Data.ArticleDefaultSeoDescription;
                            this.seoGeneralSettingUpdateDto.ArticleSchemaPageType = response.data.SeoGeneralSettingUpdateDto.Data.ArticleSchemaPageType;
                            this.seoGeneralSettingUpdateDto.ArticleSchemaArticleType = response.data.SeoGeneralSettingUpdateDto.Data.ArticleSchemaArticleType;
                            this.seoGeneralSettingUpdateDto.ArticleSocialTitle = response.data.SeoGeneralSettingUpdateDto.Data.ArticleSocialTitle;
                            this.seoGeneralSettingUpdateDto.ArticleSocialDescription = response.data.SeoGeneralSettingUpdateDto.Data.ArticleSocialDescription;
                            this.seoGeneralSettingUpdateDto.IsActiveCategorySearchEngineIndex = response.data.SeoGeneralSettingUpdateDto.Data.IsActiveCategorySearchEngineIndex;
                            this.seoGeneralSettingUpdateDto.IsActiveCategorySeoSetting = response.data.SeoGeneralSettingUpdateDto.Data.IsActiveCategorySeoSetting;
                            this.seoGeneralSettingUpdateDto.CategoryDefaultSeoTitle = response.data.SeoGeneralSettingUpdateDto.Data.CategoryDefaultSeoTitle;
                            this.seoGeneralSettingUpdateDto.CategoryDefaultSeoDescription = response.data.SeoGeneralSettingUpdateDto.Data.CategoryDefaultSeoDescription;
                            this.seoGeneralSettingUpdateDto.CategorySocialTitle = response.data.SeoGeneralSettingUpdateDto.Data.CategorySocialTitle;
                            this.seoGeneralSettingUpdateDto.CategorySocialDescription = response.data.SeoGeneralSettingUpdateDto.Data.CategorySocialDescription;
                            this.seoGeneralSettingUpdateDto.IsActiveTagSearchEngineIndex = response.data.SeoGeneralSettingUpdateDto.Data.IsActiveTagSearchEngineIndex;
                            this.seoGeneralSettingUpdateDto.IsActiveTagSeoSetting = response.data.SeoGeneralSettingUpdateDto.Data.IsActiveTagSeoSetting;
                            this.seoGeneralSettingUpdateDto.TagDefaultSeoTitle = response.data.SeoGeneralSettingUpdateDto.Data.TagDefaultSeoTitle;
                            this.seoGeneralSettingUpdateDto.TagDefaultSeoDescription = response.data.SeoGeneralSettingUpdateDto.Data.TagDefaultSeoDescription;
                            this.seoGeneralSettingUpdateDto.TagSocialTitle = response.data.SeoGeneralSettingUpdateDto.Data.TagSocialTitle;
                            this.seoGeneralSettingUpdateDto.TagSocialDescription = response.data.SeoGeneralSettingUpdateDto.Data.TagSocialDescription;
                            this.seoGeneralSettingUpdateDto.FacebookPageLink = response.data.SeoGeneralSettingUpdateDto.Data.FacebookPageLink;
                            this.seoGeneralSettingUpdateDto.TwitterUserName = response.data.SeoGeneralSettingUpdateDto.Data.TwitterUserName;
                            this.seoGeneralSettingUpdateDto.InstagramProfilLink = response.data.SeoGeneralSettingUpdateDto.Data.InstagramProfilLink;
                            this.seoGeneralSettingUpdateDto.LinkedInProfilLink = response.data.SeoGeneralSettingUpdateDto.Data.LinkedInProfilLink;
                            this.seoGeneralSettingUpdateDto.PinterestProfilLink = response.data.SeoGeneralSettingUpdateDto.Data.PinterestProfilLink;
                            this.seoGeneralSettingUpdateDto.YoutubeChannelLink = response.data.SeoGeneralSettingUpdateDto.Data.YoutubeChannelLink;
                            this.seoGeneralSettingUpdateDto.WikipediaLink = response.data.SeoGeneralSettingUpdateDto.Data.WikipediaLink;
                            this.seoGeneralSettingUpdateDto.IsActiveOpenGraph = response.data.SeoGeneralSettingUpdateDto.Data.IsActiveOpenGraph;
                            this.seoGeneralSettingUpdateDto.IsActiveTwitterOpenGraph = response.data.SeoGeneralSettingUpdateDto.Data.IsActiveTwitterOpenGraph;
                            this.seoGeneralSettingUpdateDto.TwitterCardType = response.data.SeoGeneralSettingUpdateDto.Data.TwitterCardType;
                            this.seoGeneralSettingUpdateDto.PinterestConfirmationCode = response.data.SeoGeneralSettingUpdateDto.Data.PinterestConfirmationCode;

                            if (response.data.SeoGeneralSettingUpdateDto.Data.SiteMainImage != null) {
                                this.siteMainImage.id = response.data.SeoGeneralSettingUpdateDto.Data.SiteMainImage.Id
                                this.siteMainImage.fileName = response.data.SeoGeneralSettingUpdateDto.Data.SiteMainImage.FileName
                                this.siteMainImage.altText = response.data.SeoGeneralSettingUpdateDto.Data.SiteMainImage.AltText
                            }

                            if (response.data.SeoGeneralSettingUpdateDto.Data.PageSocialImage != null) {
                                this.pageSocialImage.id = response.data.SeoGeneralSettingUpdateDto.Data.PageSocialImage.Id
                                this.pageSocialImage.fileName = response.data.SeoGeneralSettingUpdateDto.Data.PageSocialImage.FileName
                                this.pageSocialImage.altText = response.data.SeoGeneralSettingUpdateDto.Data.PageSocialImage.AltText
                            }

                            if (response.data.SeoGeneralSettingUpdateDto.Data.ArticleSocialImage != null) {
                                this.articleSocialImage.id = response.data.SeoGeneralSettingUpdateDto.Data.ArticleSocialImage.Id
                                this.articleSocialImage.fileName = response.data.SeoGeneralSettingUpdateDto.Data.ArticleSocialImage.FileName
                                this.articleSocialImage.altText = response.data.SeoGeneralSettingUpdateDto.Data.ArticleSocialImage.AltText
                            }

                            if (response.data.SeoGeneralSettingUpdateDto.Data.CategorySocialImage != null) {
                                this.categorySocialImage.id = response.data.SeoGeneralSettingUpdateDto.Data.CategorySocialImage.Id
                                this.categorySocialImage.fileName = response.data.SeoGeneralSettingUpdateDto.Data.CategorySocialImage.FileName
                                this.categorySocialImage.altText = response.data.SeoGeneralSettingUpdateDto.Data.CategorySocialImage.AltText
                            }

                            if (response.data.SeoGeneralSettingUpdateDto.Data.TagSocialImage != null) {
                                this.tagSocialImage.id = response.data.SeoGeneralSettingUpdateDto.Data.TagSocialImage.Id
                                this.tagSocialImage.fileName = response.data.SeoGeneralSettingUpdateDto.Data.TagSocialImage.FileName
                                this.tagSocialImage.altText = response.data.SeoGeneralSettingUpdateDto.Data.TagSocialImage.AltText
                            }

                            if (response.data.SeoGeneralSettingUpdateDto.Data.OpenGraphImage != null) {
                                this.openGraphImage.id = response.data.SeoGeneralSettingUpdateDto.Data.OpenGraphImage.Id
                                this.openGraphImage.fileName = response.data.SeoGeneralSettingUpdateDto.Data.OpenGraphImage.FileName
                                this.openGraphImage.altText = response.data.SeoGeneralSettingUpdateDto.Data.OpenGraphImage.AltText
                            }
                            this.overlayShow = false;
                        }
                    }).catch((error) => {
                        this.$toast({
                            component: ToastificationContent,
                            props: {
                                variant: 'danger',
                                title: 'Hata Oluştu!',
                                icon: 'AlertOctagonIcon',
                                text: 'SEO ayarları yüklenirken hata oluştu. Lütfen tekrar deneyin.',
                            }
                        })
                    });
            },
            updateData() {
                this.seoGeneralSettingUpdateDto.SiteMainImageId = this.siteMainImage.id;
                this.seoGeneralSettingUpdateDto.ArticleSocialImageId = this.pageSocialImage.id;
                this.seoGeneralSettingUpdateDto.PageSocialImageId = this.articleSocialImage.id;
                this.seoGeneralSettingUpdateDto.CategorySocialImageId = this.categorySocialImage.id;
                this.seoGeneralSettingUpdateDto.TagSocialImageId = this.tagSocialImage.id;
                this.seoGeneralSettingUpdateDto.OpenGraphImageId = this.openGraphImage.id;
                if (this.overlayShow === false) {
                    this.overlayShow = true;
                    axios.post('/admin/settings-seogeneral',
                        {
                            SeoGeneralSettingUpdateDto: this.seoGeneralSettingUpdateDto,
                        }).then((response) => {
                            if (response.data.SeoGeneralSettingDto.ResultStatus === 0) {
                                this.$toast({
                                    component: ToastificationContent,
                                    props: {
                                        variant: 'success',
                                        title: 'Başarılı İşlem!',
                                        icon: 'CheckIcon',
                                        text: response.data.SeoGeneralSettingDto.Message
                                    }
                                });
                                this.overlayShow = false;
                            }
                            else {
                                this.$toast({
                                    component: ToastificationContent,
                                    props: {
                                        variant: 'danger',
                                        title: 'Başarısız İşlem!',
                                        icon: 'AlertOctagonIcon',
                                        text: response.data.SeoGeneralSettingDto.Message
                                    },
                                })
                            }
                        }).catch((error) => {
                            this.$toast({
                                component: ToastificationContent,
                                props: {
                                    variant: 'danger',
                                    title: 'Hata Oluştu!',
                                    icon: 'AlertOctagonIcon',
                                    text: 'Veriler güncellenirken hata oluştu. Lütfen tekrar deneyiniz.',
                                },
                            })
                        });
                }
            }
        },
        mounted() {
            this.getSeoType();
            this.getSchemaPageType();
            this.getSchemaArticleType();
            this.getTwitterCardType();
            this.getAllData();
        }
    }
</script>

<style lang="scss">
    @import '@core/scss/vue/libs/vue-select.scss';

    #tab-seo-settings .nav.nav-tabs {
        background-color: #ffffff;
        box-shadow: 0 4px 24px 0 rgb(34 41 47 / 10%);
        padding: 10px 0 10px 10px;
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