<template>
    <div v-if="doHaveData === true">
        <b-row>
            <modal-media v-bind:show="modalShow"
                         @changeImage="imageChange"
                         ref="modalMedia"></modal-media>
            <b-col class="content-header-left mb-2"
                   cols="12"
                   md="7">
                <h2 class="content-header-title float-left pr-1 mb-0">
                    Etiket Düzenle
                </h2>
                <div class="breadcrumb-wrapper">
                    <b-breadcrumb>
                        <b-breadcrumb-item to="/">
                            <feather-icon icon="HomeIcon"
                                          size="16"
                                          class="align-text-top" />
                        </b-breadcrumb-item>
                        <b-breadcrumb-item :to="{ name: 'pages-tag-list' }">Etiketler</b-breadcrumb-item>
                        <b-breadcrumb-item active>Düzenle</b-breadcrumb-item>
                        <b-button v-if="$can('create','Tag')"
                                  v-b-tooltip.hover
                                  title="Yeni Etiket Ekle"
                                  v-ripple.400="'rgba(113, 102, 240, 0.15)'"
                                  variant="outline-primary"
                                  size="sm"
                                  :to="{ name:'pages-tag-list'}"
                                  class=" ml-1">Yeni Ekle</b-button>
                    </b-breadcrumb>
                </div>
            </b-col>
            <b-col class="content-header-right text-md-right d-md-block d-none mb-1"
                   md="5"
                   cols="12">
                <b-spinner v-if="buttonDisabled"
                           variant="secondary"
                           class="align-middle mr-1" />
                <b-button v-if="$can('delete','Tag')"
                          :disabled="buttonDisabled"
                          :variant="deleteButtonVariant"                          
                          class="mr-1"
                          size="sm"
                          type="button"
                          @click="deleteData">
                    Kalıcı Sil
                </b-button>
                <b-button :disabled="buttonDisabled"
                          variant="outline-primary"
                          class="mr-1"
                          size="sm"
                          type="button"
                          :to="{ name: 'pages-tag-view', params: { slug: termUpdateDto.Slug } }" target="_blank">
                    Görüntüle
                </b-button>                
                <b-button :disabled="buttonDisabled"
                          :variant="saveButtonVariant"
                          type="submit"
                          @click.prevent="validationForm">
                    Güncelle
                </b-button>
            </b-col>
            <b-col md="12"
                   lg="4">
                <b-overlay :show="showOverlay"
                           size="sm">
                    <b-card>
                        <validation-observer ref="simpleRules">
                            <b-form>
                                <b-row>
                                    <b-col cols="12">
                                        <b-form-group label="Etiket Adı"
                                                      label-for="name"
                                                      description="Sitenizde gösterilecek olan isim.">
                                            <validation-provider #default="{ errors }"
                                                                 name="name"
                                                                 vid="name"
                                                                 rules="required">
                                                <b-form-input id="name"
                                                              v-model="termUpdateDto.Name"
                                                              :state="errors.length > 0 ? false:null"
                                                              type="text"
                                                              placeholder="Etiket Adı" />
                                                <small class="text-danger">{{ errors[0] }}</small>
                                            </validation-provider>
                                        </b-form-group>
                                        <b-form-group label="Kısa İsim"
                                                      label-for="slug"
                                                      description="'slug' yazı isminin URL versiyonudur. Genellikle tümü küçük harflerden oluşur, sadece harf, rakam ve tire içerir.">
                                            <b-form-input id="slug"
                                                          v-model="termUpdateDto.Slug"
                                                          type="text"
                                                          placeholder="Kısa İsim"
                                                          @blur="changeSlug" />
                                        </b-form-group>
                                        <b-form-group label="Açıklama"
                                                      label-for="description">
                                            <b-form-textarea id="description"
                                                             v-model="termUpdateDto.Description"
                                                             placeholder="Açıklama"
                                                             rows="3" />
                                        </b-form-group>
                                    </b-col>
                                </b-row>
                            </b-form>
                        </validation-observer>
                    </b-card>
                </b-overlay>
            </b-col>
            <b-col v-if="$can('update','Seo')"
                   md="12"
                   lg="8">
                <b-overlay :show="showOverlay"
                           size="sm">
                    <b-card title="SEO Ayarları">
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

                                            <b-form-group label-for="CanonicalUrl"
                                                          description="Geçerli terim ile benzer içeriğe sahip olan terimin linkini giriniz.">
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
                                        </b-col>
                                    </b-row>
                                </b-form>
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
                                        <b-form-group class="mt-2">
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
                                        <b-form-group class="mt-2">
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
                    </b-card>
                </b-overlay>
            </b-col>
        </b-row>
    </div>
    <div v-else-if="doHaveData === false"
         class="error-message">
        Mevcut olmayan bir ögeyi düzenlemeye çalıştınız. Belki daha önceden silinmiş olabilir mi?
    </div>
</template>

<script>
    import UrlHelper from '@/helper/url-helper';
    import ModalMedia from '../../media/ModalMedia.vue';
    import ToastificationContent from '@core/components/toastification/ToastificationContent.vue';
    import AppCollapse from '@core/components/app-collapse/AppCollapse.vue';
    import AppCollapseItem from '@core/components/app-collapse/AppCollapseItem.vue';
    import { ValidationProvider, ValidationObserver, extend } from 'vee-validate';
    import { required } from '@validations';

    import {
        BRow, BCol, BBreadcrumb, BBreadcrumbItem, BSpinner, BButton, BOverlay, BCard, BForm, BFormGroup, BFormInput, BFormCheckbox, BFormTextarea, BFormTags, BTabs, BTab, BImg, VBModal, VBTooltip
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
            AppCollapse,
            AppCollapseItem,
            BRow,
            BCol,
            BBreadcrumb,
            BBreadcrumbItem,
            BSpinner,
            BButton,
            BOverlay,
            BCard,
            BForm,
            BFormGroup,
            BFormInput,
            BFormCheckbox,
            BFormTextarea,
            BFormTags,
            BTabs,
            BTab,
            BImg
        },
        props: {
            selectedRadio: Number,
            selectedFileName: String,
        },
        directives: {
            'b-modal': VBModal,
            'b-tooltip': VBTooltip,
            Ripple,
        },
        data() {
            return {
                buttonDisabled: false,
                saveButtonVariant: 'primary',
                deleteButtonVariant: 'outline-danger',
                doHaveData: Boolean,
                showOverlay: false,
                required,
                terms: [],
                fields: [{ key: 'Id', sortable: false, thStyle: { width: "20px", padding: "0.72rem !important" } }, { key: 'Name', label: 'İSİM', sortable: true, thStyle: { padding: "0.72rem  !important" } }, { key: 'Slug', label: 'KISA İSİM', sortable: true, thStyle: { width: "50px", padding: "0.72rem  !important" } }, { key: 'Count', label: 'Toplam', sortable: true, thStyle: { width: "80px", padding: "0.72rem  !important", textalign: "center" } }],
                termUpdateDto: {
                    Id: this.$route.query.edit,
                    Name: '',
                    Slug: '',
                    ParentId: null,
                    Description: '',
                    TermType: '',
                },
                seoObjectSettingUpdateDto: {
                    Id: '',
                    SeoTitle: '',
                    FocusKeyword: '',
                    SeoDescription: '',
                    CanonicalUrl: '',
                    IsRobotsNoIndex: false,
                    OpenGraphImageId: '',
                    OpenGraphTitle: '',
                    OpenGraphDescription: '',
                    TwitterImageId: '',
                    TwitterTitle: '',
                    TwitterDescription: '',
                },
                keywords: [],
                modalShow: false,
                noImage: require('@/assets/images/default/default-post-image.jpg'),
                openGraphImage: {
                    id: null,
                    fileName: null,
                    altText: null,
                },
                twitterImage: {
                    id: null,
                    fileName: null,
                    altText: null
                },
                isOpenGraphImageChoose: false,
                isTwitterImageChoose: false,
            }
        },
        methods: {
            getData() {
                this.showOverlay = true;
                axios.get('/admin/term-edit?termId=' + this.$route.query.edit)
                    .then((response) => {
                        if (response.data.TermUpdateDto.ResultStatus === 0) {
                            if (response.data.TermUpdateDto.Data.TermType !== 3) {
                                this.doHaveData = false;
                            } else {
                                this.doHaveData = true;
                                this.termUpdateDto.Name = response.data.TermUpdateDto.Data.Name;
                                this.termUpdateDto.Slug = response.data.TermUpdateDto.Data.Slug;

                                this.termUpdateDto.ParentId = response.data.TermUpdateDto.Data.ParentId;
                                this.termUpdateDto.Description = response.data.TermUpdateDto.Data.Description;
                                this.termUpdateDto.TermType = response.data.TermUpdateDto.Data.TermType;

                                this.seoObjectSettingUpdateDto.Id = response.data.SeoObjectSettingUpdateDto.Data.Id;
                                this.seoObjectSettingUpdateDto.SeoTitle = response.data.SeoObjectSettingUpdateDto.Data.SeoTitle;
                                this.seoObjectSettingUpdateDto.SeoDescription = response.data.SeoObjectSettingUpdateDto.Data.SeoDescription;
                                this.seoObjectSettingUpdateDto.CanonicalUrl = response.data.SeoObjectSettingUpdateDto.Data.CanonicalUrl;
                                this.seoObjectSettingUpdateDto.IsRobotsNoIndex = response.data.SeoObjectSettingUpdateDto.Data.IsRobotsNoIndex;


                                this.seoObjectSettingUpdateDto.OpenGraphTitle = response.data.SeoObjectSettingUpdateDto.Data.OpenGraphTitle;
                                this.seoObjectSettingUpdateDto.OpenGraphDescription = response.data.SeoObjectSettingUpdateDto.Data.OpenGraphDescription;

                                this.seoObjectSettingUpdateDto.TwitterTitle = response.data.SeoObjectSettingUpdateDto.Data.TwitterTitle;
                                this.seoObjectSettingUpdateDto.TwitterDescription = response.data.SeoObjectSettingUpdateDto.Data.TwitterDescription;

                                this.keywords = response.data.SeoObjectSettingUpdateDto.Data.FocusKeyword == null ? [] : response.data.SeoObjectSettingUpdateDto.Data.FocusKeyword.split(',');

                                if (response.data.SeoObjectSettingUpdateDto.Data.OpenGraphImage != null) {
                                    this.openGraphImage.id = response.data.SeoObjectSettingUpdateDto.Data.OpenGraphImageId;
                                    this.openGraphImage.fileName = response.data.SeoObjectSettingUpdateDto.Data.OpenGraphImage.FileName;
                                    this.openGraphImage.altText = response.data.SeoObjectSettingUpdateDto.Data.OpenGraphImage.AltText;
                                }

                                if (response.data.SeoObjectSettingUpdateDto.Data.TwitterImage != null) {
                                    this.twitterImage.id = response.data.SeoObjectSettingUpdateDto.Data.TwitterImageId;
                                    this.twitterImage.fileName = response.data.SeoObjectSettingUpdateDto.Data.TwitterImage.FileName;
                                    this.twitterImage.altText = response.data.SeoObjectSettingUpdateDto.Data.TwitterImage.AltText;
                                }
                                this.showOverlay = false;
                            }
                        }
                        else {
                            this.doHaveData = false;
                        }
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
            changeSlug() {
                var seoSlug = UrlHelper.friendlySEOUrl(this.termUpdateDto.Slug);
                this.termUpdateDto.Slug = seoSlug;
            },
            imageChange(id, name, altText) {
                if (this.isOpenGraphImageChoose == true) {
                    this.openGraphImage.id = id;
                    this.openGraphImage.fileName = name;
                    this.openGraphImage.altText = altText;
                } else if (this.isTwitterImageChoose == true) {
                    this.twitterImage.id = id;
                    this.twitterImage.fileName = name;
                    this.twitterImage.altText = altText;
                }
                this.isOpenGraphImageChoose = false;
                this.isTwitterImageChoose = false;
            },
            selectImage: function (e) {
                if (e.target._prevClass.includes('select-opengraph-image')) {
                    this.isOpenGraphImageChoose = true;
                } else if (e.target._prevClass.includes('select-twitter-image')) {
                    this.isTwitterImageChoose = true;
                }
            },
            removeImage: function (e) {
                if (e.target._prevClass.includes('remove-opengraph-image')) {
                    this.openGraphImage.id = null;
                    this.openGraphImage.fileName = null;
                    this.openGraphImage.altText = null;
                } else if (e.target._prevClass.includes('remove-twitter-image')) {
                    this.twitterImage.id = null;
                    this.twitterImage.fileName = null;
                    this.twitterImage.altText = null;
                }
            },
            validationForm() {
                this.buttonDisabled = true;
                this.saveButtonVariant = 'outline-secondary';
                this.$refs.simpleRules.validate().then(success => {
                    if (success) {
                        this.seoObjectSettingUpdateDto.FocusKeyword = this.keywords.toString();
                        this.seoObjectSettingUpdateDto.OpenGraphImageId = this.openGraphImage.id;
                        this.seoObjectSettingUpdateDto.TwitterImageId = this.twitterImage.id;
                        axios.post('/admin/term-edit',
                            {
                                TermUpdateDto: this.termUpdateDto,
                                SeoObjectSettingUpdateDto: this.seoObjectSettingUpdateDto
                            })
                            .then((response) => {
                                if (response.data.TermDto.ResultStatus == 0) {
                                    this.$toast({
                                        component: ToastificationContent,
                                        props: {
                                            variant: 'success',
                                            title: 'Başarılı İşlem!',
                                            icon: 'CheckIcon',
                                            text: response.data.TermDto.Message
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
                                            text: response.data.TermDto.Message,
                                        },
                                    })
                                }
                                this.buttonDisabled = false;
                                this.saveButtonVariant = 'primary';
                            })
                            .catch((error) => {
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
                    }
                })
            },
            deleteData() {
                this.buttonDisabled = true;
                this.deleteButtonVariant = 'outline-secondary';
                this.$swal({
                    title: 'Silmek istediğinize emin misiniz?',
                    text: this.termUpdateDto.Name + " isimli terim kalıcı olarak silinecektir?",
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
                        axios.post('/admin/term-delete?termId=' + this.termUpdateDto.Id)
                            .then((response) => {
                                if (response.data.ResultStatus === 0) {
                                    this.$router.push({ path: '/admin/tags' });

                                    this.$toast({
                                        component: ToastificationContent,
                                        props: {
                                            variant: 'success',
                                            title: 'Başarılı İşlem!',
                                            icon: 'CheckIcon',
                                            text: response.data.Message,
                                        }
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
                    this.buttonDisabled = false;
                    this.deleteButtonVariant = 'outline-danger';
                })
            },
        },
        mounted() {
            this.getData();
        }
    }
</script>

<style lang="scss">
    .error-message {
        background: #ffffff !important;
        padding: 25px;
        text-align: center;
        border-radius: 5px;
        -webkit-box-shadow: 0px 0px 3px 0px rgba(196,196,196,1);
        -moz-box-shadow: 0px 0px 3px 0px rgba(196,196,196,1);
        box-shadow: 0px 0px 3px 0px rgba(196,196,196,1);
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