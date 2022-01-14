<template>
    <div v-if="doHaveData === true">
        <b-row>
            <modal-media v-bind:show="modalShow"
                         @changeImage="imageChange"
                         ref="modalMedia"></modal-media>
            <b-col class="content-header-left mb-2"
                   cols="12"
                   md="8">
                <h2 class="content-header-title float-left pr-1 mb-0">
                    {{ title }}
                </h2>
                <div class="breadcrumb-wrapper">
                    <b-breadcrumb>
                        <b-breadcrumb-item to="/">
                            <feather-icon icon="HomeIcon"
                                          size="16"
                                          class="align-text-top" />
                        </b-breadcrumb-item>
                        <b-breadcrumb-item v-if="isParent == true"
                                           :to="{ name: 'pages-category-list' }">Kategoriler</b-breadcrumb-item>
                        <b-breadcrumb-item v-else
                                           :to="{ name: 'pages-tag-list' }">Etiketler</b-breadcrumb-item>
                        <b-breadcrumb-item active>Düzenle</b-breadcrumb-item>
                        <b-button v-b-tooltip.hover
                                  :title="tooltipText"
                                  v-ripple.400="'rgba(113, 102, 240, 0.15)'"
                                  variant="outline-primary"
                                  size="sm"
                                  :to="{ name:'pages-article-add'}"
                                  class=" ml-1">Yeni Ekle</b-button>
                    </b-breadcrumb>
                </div>
            </b-col>
            <b-col class="content-header-right text-md-right d-md-block d-none mb-1"
                   md="4"
                   cols="12">
                <b-button variant="flat-danger"
                          class="mr-1"
                          size="sm"
                          type="button"
                          @click="deleteData">
                    Kalıcı Sil
                </b-button>
                <b-button v-if="termUpdateDto.TermType == 2 || termUpdateDto.TermType == 'category'"
                          variant="outline-primary"
                          class="mr-1"
                          size="sm"
                          type="button"
                          :to=" {name: 'pages-category-view', params: { slug: termUpdateDto.Slug }}">
                    Görüntüle
                </b-button>
                <b-button v-else-if="termUpdateDto.TermType == 3 || termUpdateDto.TermType == 'tag'"
                          variant="outline-primary"
                          class="mr-1"
                          size="sm"
                          type="button"
                          :to=" {name: 'pages-tag-view', params: { slug: termUpdateDto.Slug }}">
                    Görüntüle
                </b-button>
                <b-button variant="primary"
                          type="submit"
                          @click.prevent="validationForm">
                    Güncelle
                </b-button>
            </b-col>
            <b-col md="12"
                   lg="4">
                <b-card>
                    <validation-observer ref="simpleRules">
                        <b-form>
                            <b-row>
                                <b-col cols="12">
                                    <b-form-group label-for="Name"
                                                  description="Sitenizde gösterilecek olan isim.">
                                        <validation-provider #default="{ errors }"
                                                             name="Name"
                                                             vid="Name"
                                                             rules="required">
                                            <b-form-input id="Name"
                                                          v-model="termUpdateDto.Name"
                                                          :state="errors.length > 0 ? false:null"
                                                          type="text"
                                                          :placeholder="namePlaceholder" />
                                            <small class="text-danger">{{ errors[0] }}</small>
                                        </validation-provider>
                                    </b-form-group>

                                    <b-form-group label-for="Slug"
                                                  description="'slug' yazı isminin URL versiyonudur. Genellikle tümü küçük harflerden oluşur, sadece harf, rakam ve tire içerir.">
                                        <b-form-input id="Slug"
                                                      v-model="termUpdateDto.Slug"
                                                      type="text"
                                                      placeholder="Kısa İsim" />
                                    </b-form-group>

                                    <b-form-group v-show="isParent"
                                                  label-for="parentTerms"
                                                  description="Mevcut kategori için üst kategoriyi buradan seçebilirsiniz.">
                                        <v-select id="parentTerms"
                                                  v-model="selected"
                                                  :options="allParentTerms"
                                                  label="Name"
                                                  :reduce="(option) => option.Id"
                                                  placeholder="Üst Kategori Seçiniz..."
                                                  @input="onChangeMethod($event)" />
                                    </b-form-group>

                                    <b-form-textarea id="description"
                                                     v-model="termUpdateDto.Description"
                                                     placeholder="Açıklama"
                                                     rows="3" />
                                </b-col>
                            </b-row>
                        </b-form>
                    </validation-observer>
                </b-card>
            </b-col>
            <b-col md="12"
                   lg="8">
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
                </b-card>
            </b-col>
        </b-row>
    </div>
    <div v-else-if="doHaveData === false"
         class="error-message">
        Mevcut olmayan bir ögeyi düzenlemeye çalıştınız. Belki daha önceden silinmiş olabilir mi?
    </div>
</template>

<script>
    import BCardActions from '@core/components/b-card-actions/BCardActions.vue';
    import { ValidationProvider, ValidationObserver, extend } from 'vee-validate';
    import required from '@validations';
    import {
        BBreadcrumb, BBreadcrumbItem, BModal, BCollapse, VBModal, BImg, BFormCheckbox, BButton, BCard, BCardBody, BCardTitle, BCardText, BRow, BCol, BForm, BFormGroup, BFormInput, BFormSelect, BFormTextarea, VBTooltip, BLink, BTabs, BTab, BFormTags, BMedia, BMediaAside, BMediaBody
    } from 'bootstrap-vue';
    //import { codeRowDetailsSupport } from './code'
    import axios from 'axios';
    import ToastificationContent from '@core/components/toastification/ToastificationContent.vue';
    import vSelect from 'vue-select';
    import Ripple from 'vue-ripple-directive';
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
            BImg,
            ModalMedia,
            BModal,
            BCardActions,
            BCardTitle,
            BCardText,
            BForm,
            BFormTags,
            BButton,
            BFormCheckbox,
            BFormTextarea,
            BCard,
            BRow,
            BCol,
            BCardBody,
            BFormGroup,
            BFormInput,
            BFormSelect,
            ToastificationContent,
            ValidationProvider,
            ValidationObserver,
            vSelect,
            BLink,
            BTabs,
            BTab,
            BMedia,
            BMediaAside,
            BMediaBody
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
                doHaveData: Boolean,
                required,
                terms: [],
                allParentTerms: [],
                isParent: false,
                selected: '',
                selectedValue: null,
                fields: [{ key: 'Id', sortable: false, thStyle: { width: "20px", padding: "0.72rem !important" } }, { key: 'Name', label: 'İSİM', sortable: true, thStyle: { padding: "0.72rem  !important" } }, { key: 'Slug', label: 'KISA İSİM', sortable: true, thStyle: { width: "50px", padding: "0.72rem  !important" } }, { key: 'Count', label: 'Toplam', sortable: true, thStyle: { width: "80px", padding: "0.72rem  !important", textalign: "center" } }],
                title: '',
                namePlaceholder: '',
                tooltipText: '',
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
                if (e.target.id == "selectOpenGraphImage") {
                    this.isOpenGraphImageChoose = true;
                } else if (e.target.id == "selectTwitterImage") {
                    this.isTwitterImageChoose = true;
                }
                this.modalShow = true;
            },
            removeImage: function (e) {
                if (e.target.id == "removeOpenGraphImage") {
                    this.openGraphImage.id = null;
                    this.openGraphImage.fileName = null;
                    this.openGraphImage.altText = null;
                } else if (e.target.id == "removeTwitterImage") {
                    this.twitterImage.id = null;
                    this.twitterImage.fileName = null;
                    this.twitterImage.altText = null;
                }
            },
            onChangeMethod(value) {
                this.termUpdateDto.ParentId = value;
            },
            validationForm() {
                this.$refs.simpleRules.validate().then(success => {
                    if (success) {
                        this.seoObjectSettingUpdateDto.FocusKeyword = this.keywords.toString();
                        this.seoObjectSettingUpdateDto.OpenGraphImageId = this.openGraphImage.id;
                        this.seoObjectSettingUpdateDto.TwitterImageId = this.twitterImage.id;
                        axios.post('/admin/term/edit',
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
                        axios.post('/admin/term/delete?term=' + this.termUpdateDto.Id)
                            .then((response) => {
                                if (response.data.ResultStatus === 0) {
                                    if (this.termUpdateDto.TermType === 2) {
                                        this.$router.push({ path: '/admin/categories' });
                                    }
                                    else if (this.termUpdateDto.TermType === 3) {
                                        this.$router.push({ path: '/admin/tags' });
                                    }
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
                })
            },
            getData() {
                axios.get('/admin/term/edit?term=' + this.$route.query.edit)
                    .then((response) => {
                        console.log(response.data);
                        if (response.data.TermUpdateDto != null) {
                            this.doHaveData = true;
                            this.termUpdateDto.Name = response.data.TermUpdateDto.Name;
                            this.termUpdateDto.Slug = response.data.TermUpdateDto.Slug;

                            this.termUpdateDto.ParentId = response.data.TermUpdateDto.ParentId;
                            this.termUpdateDto.Description = response.data.TermUpdateDto.Description;
                            this.termUpdateDto.TermType = response.data.TermUpdateDto.TermType;

                            this.seoObjectSettingUpdateDto.Id = response.data.SeoObjectSettingUpdateDto.Id;
                            this.seoObjectSettingUpdateDto.SeoTitle = response.data.SeoObjectSettingUpdateDto.SeoTitle;
                            this.seoObjectSettingUpdateDto.SeoDescription = response.data.SeoObjectSettingUpdateDto.SeoDescription;
                            this.seoObjectSettingUpdateDto.CanonicalUrl = response.data.SeoObjectSettingUpdateDto.CanonicalUrl;
                            this.seoObjectSettingUpdateDto.IsRobotsNoIndex = response.data.SeoObjectSettingUpdateDto.IsRobotsNoIndex;


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

                            if (response.data.TermUpdateDto.TermType === 2) {
                                this.isParent = true;
                                if (response.data.TermUpdateDto.Parent != null) {
                                    this.selected = {
                                        Id: response.data.TermUpdateDto.Parent.Id,
                                        Name: response.data.TermUpdateDto.Parent.Name,
                                    }
                                }

                                this.title = "Kategoriyi Düzenle";
                                this.namePlaceholder = "Kategori Adı";
                                this.tooltipText = "Yeni Kategori Ekle";
                            }
                            else if (response.data.TermUpdateDto.TermType === 3) {
                                this.title = "Etiketi Düzenle";
                                this.namePlaceholder = "Etiket Adı";
                                this.tooltipText = "Yeni Etiket Ekle";
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
            getParentList() {
                axios.get('/admin/term/getparentlist?termId=' + this.$route.query.edit)
                    .then((response) => {
                        if (response.data.ResultStatus === 0) {
                            this.allParentTerms = response.data.Terms;
                        }
                        else {
                            this.$toast({
                                component: ToastificationContent,
                                props: {
                                    variant: 'danger',
                                    title: 'Hata Oluştu!',
                                    icon: 'AlertOctagonIcon',
                                    text: this.title + ' listelenirken hata oluştu. ',
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
                                text: 'Hata oluştu. Lütfen tekrar deneyin.',
                            }
                        })
                    });
            },
        },
        mounted() {
            this.getData();
            this.getParentList();
        }
    }
</script>

<style lang="scss">
    @import '@core/scss/vue/libs/vue-select.scss';

    .error-message {
        background: #ffffff !important;
        padding: 25px;
        text-align: center;
        border-radius: 5px;
        -webkit-box-shadow: 0px 0px 3px 0px rgba(196,196,196,1);
        -moz-box-shadow: 0px 0px 3px 0px rgba(196,196,196,1);
        box-shadow: 0px 0px 3px 0px rgba(196,196,196,1);
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