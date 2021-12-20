<template>
    <b-row>
        <modal-media v-bind:show="modalShow"
                     @changeImage="imageChange"
                     ref="modalMedia"></modal-media>
        <b-col md="12"
               lg="4">
            <b-card :title="formTitle">
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

                                <b-form-group label-for="slug"
                                              description="'slug' yazı isminin URL versiyonudur. Genellikle tümü küçük harflerden oluşur, sadece harf, rakam ve tire içerir.">
                                    <b-form-input id="slug"
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

                                <!-- reset button -->
                                <b-button variant="primary"
                                          class="float-right mt-1"
                                          type="submit"
                                          @click.prevent="validationForm">
                                    Güncelle
                                </b-button>
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

                        <b-row class="kb-search-content-info match-height">

                            <b-col lg="4"
                                   md="4"
                                   sm="6"
                                   class="kb-search-content">
                                <b-img rounded
                                       :src="require('@/assets/images/media' + openGraphImage.fileName)"
                                       :alt="openGraphImage.altText"
                                       class="d-inline-block mr-1 mb-1 w-100 h-100" />
                            </b-col>
                            <b-col lg="3"
                                   md="3"
                                   sm="6">
                                <b-button v-ripple.400="'rgba(255, 255, 255, 0.15)'"
                                          variant="primary"
                                          size="sm"
                                          class="mb-75 mr-75"
                                          v-b-modal.modal-media
                                          @click="selectedOpenGraphImage">
                                    Resim Seç
                                </b-button>
                                <!--/ upload button -->
                                <!-- reset -->
                                <b-button v-ripple.400="'rgba(186, 191, 199, 0.15)'"
                                          variant="outline-secondary"
                                          size="sm"
                                          class="mb-75 mr-75">
                                    Resmi Kaldır
                                </b-button>
                                <b-input type="text"
                                         v-model="openGraphImage.id"></b-input>
                            </b-col>
                        </b-row>

                        <b-form-group>
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
                    </b-tab>
                    <b-tab title="Twitter">
                        <b-row class="kb-search-content-info match-height">
                            <b-col lg="4"
                                   md="4"
                                   sm="6"
                                   class="kb-search-content">
                                <b-img rounded
                                       :src="require('@/assets/images/media' + twitterImage.fileName)"
                                       :alt="twitterImage.altText"
                                       class="d-inline-block mr-1 mb-1 w-100 h-100" />
                            </b-col>
                            <b-col lg="3"
                                   md="3"
                                   sm="6">
                                <b-button v-ripple.400="'rgba(255, 255, 255, 0.15)'"
                                          variant="primary"
                                          size="sm"
                                          class="mb-75 mr-75"
                                          v-b-modal.modal-media
                                          @click="selectedTwitterImage">
                                    Resim Seç
                                </b-button>
                                <!--/ upload button -->
                                <!-- reset -->
                                <b-button v-ripple.400="'rgba(186, 191, 199, 0.15)'"
                                          variant="outline-secondary"
                                          size="sm"
                                          class="mb-75 mr-75">
                                    Resmi Kaldır
                                </b-button>
                                <b-input type="text"
                                         v-model="twitterImage.id"></b-input>
                            </b-col>
                        </b-row>
                        <b-form-group>
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
                    </b-tab>
                </b-tabs>
            </b-card>
        </b-col>
    </b-row>
</template>

<script>
    import BCardActions from '@core/components/b-card-actions/BCardActions.vue'
    import { ValidationProvider, ValidationObserver, extend } from 'vee-validate'
    import { required, min, confirmed } from '@validations'
    import {
        BModal, BAvatar, VBModal, BImg, BFormCheckbox, BButton, BCard, BCardBody, BCardTitle, BCardText, BRow, BCol, BForm, BFormGroup, BFormInput, BFormSelect, BFormTextarea, VBTooltip, BLink, BTabs, BTab, BFormTags, BMedia, BMediaAside, BMediaBody
    } from 'bootstrap-vue'
    //import { codeRowDetailsSupport } from './code'
    import axios from 'axios'
    import ToastificationContent from '@core/components/toastification/ToastificationContent.vue'
    import vSelect from 'vue-select'
    import Ripple from 'vue-ripple-directive'
    import ModalMedia from '../media/ModalMedia.vue'

    extend('required', {
        ...required,
        message: 'Lütfen gerekli bilgileri yazınız.'
    });

    export default {
        components: {
            BImg,
            BAvatar,
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
                required,
                min,
                confirmed,
                terms: [],
                allParentTerms: [],
                isParent: false,
                selected: '',
                selectedValue: null,
                name: "",
                fields: [{ key: 'Id', sortable: false, thStyle: { width: "20px", padding: "0.72rem !important" } }, { key: 'Name', label: 'İSİM', sortable: true, thStyle: { padding: "0.72rem  !important" } }, { key: 'Slug', label: 'KISA İSİM', sortable: true, thStyle: { width: "50px", padding: "0.72rem  !important" } }, { key: 'Count', label: 'Toplam', sortable: true, thStyle: { width: "80px", padding: "0.72rem  !important", textalign: "center" } }],
                formTitle: "",
                namePlaceholder: "",
                termUpdateDto: {
                    Id: this.$route.query.edit,
                    Name: "",
                    Slug: "",
                    ParentId: null,
                    Description: "",
                    TermType: "",
                },
                seoObjectSettingUpdateDto: {
                    SeoTitle: "",
                    SeoDescription: "",
                    CanonicalUrl: "",
                    IsRobotsNoIndex: false,
                    OpenGraphImageId: "",
                    OpenGraphTitle: "",
                    OpenGraphDescription: "",
                    TwitterImageId: "",
                    TwitterTitle: "",
                    TwitterDescription: "",
                },
                keywords: [],
                openGraphImage: "",
                twitterImage: "",
                modalShow: false,
                openGraphImage: {
                    id: 1,
                    fileName: '1',
                    altText: '1'
                },
                twitterImage: {
                    id: 1,
                    fileName: '1',
                    altText: '1'
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
            selectedOpenGraphImage() {
                this.isOpenGraphImageChoose = true;
                this.modalShow = true;
            },
            selectedTwitterImage(twitterImage) {
                this.isTwitterImageChoose = true;
                this.modalShow = true;
            },
            onChangeMethod(value) {
                this.termUpdateDto.ParentId = value;
            },
            validationForm() {
                this.$refs.simpleRules.validate().then(success => {
                    if (success) {
                        axios.post('/admin/term/edit',
                            {
                                TermUpdateDto: this.termUpdateDto,
                                SeoObjectSettingUpdateDto: this.seoObjectSettingUpdateDto
                            })
                            .then((response) => {
                                if (response.data.termResult.ResultStatus === 0) {
                                    this.$toast({
                                        component: ToastificationContent,
                                        props: {
                                            variant: 'success',
                                            title: 'Başarılı İşlem!',
                                            icon: 'CheckIcon',
                                            text: response.data.termResult.Message
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
                                    },
                                })
                            });
                    }
                })
            },
            singleDeleteData(id, name) {
                this.$swal({
                    title: 'Silmek istediğinize emin misiniz?',
                    text: name + " isimli terim kalıcı olarak silinecektir?",
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
                        axios.post('/admin/term/delete',
                            {
                                term: id
                            })
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
                            })
                            .catch(function (error) {
                                this.$toast({
                                    component: ToastificationContent,
                                    props: {
                                        variant: 'danger',
                                        title: 'Hata Oluştu!',
                                        icon: 'AlertOctagonIcon',
                                        text: error,
                                    },
                                })
                            });
                    }
                })
            },
            getData() {
                console.log(ModalMedia.selectedRadio)
                axios.get('/admin/term/edit?term=' + this.$route.query.edit)
                    .then((response) => {
                        if (response.data.termResult.ResultStatus === 0) {
                            this.termUpdateDto.Name = response.data.termResult.Data.Name;
                            this.termUpdateDto.Slug = response.data.termResult.Data.Slug;

                            this.termUpdateDto.ParentId = response.data.termResult.Data.ParentId;
                            this.termUpdateDto.Description = response.data.termResult.Data.Description;
                            this.termUpdateDto.TermType = response.data.termResult.Data.TermType;

                            this.seoObjectSettingUpdateDto.SeoTitle = response.data.seoResult.Data.SeoTitle;
                            this.seoObjectSettingUpdateDto.SeoDescription = response.data.seoResult.Data.SeoDescription;
                            this.seoObjectSettingUpdateDto.CanonicalUrl = response.data.seoResult.Data.CanonicalUrl;
                            this.seoObjectSettingUpdateDto.IsRobotsNoIndex = response.data.seoResult.Data.IsRobotsNoIndex;
                            this.seoObjectSettingUpdateDto.OpenGraphImageId = response.data.seoResult.Data.OpenGraphImageId;
                            this.seoObjectSettingUpdateDto.OpenGraphTitle = response.data.seoResult.Data.OpenGraphTitle;
                            this.seoObjectSettingUpdateDto.OpenGraphDescription = response.data.seoResult.Data.OpenGraphDescription;
                            this.seoObjectSettingUpdateDto.TwitterImageId = response.data.seoResult.Data.TwitterImageId;
                            this.seoObjectSettingUpdateDto.TwitterTitle = response.data.seoResult.Data.TwitterTitle;
                            this.seoObjectSettingUpdateDto.TwitterDescription = response.data.seoResult.Data.TwitterDescription;

                            this.keywords = response.data.seoResult.Data.FocusKeyword == null ? [] : response.data.seoResult.Data.FocusKeyword.split(',');

                            if (response.data.seoResult.Data.OpenGraphImage != null) {
                                this.openGraphImage = response.data.seoResult.Data.OpenGraphImage.FileName;
                            }

                            if (response.data.seoResult.Data.TwitterImage != null) {
                                this.twitterImage = response.data.seoResult.Data.TwitterImage.FileName;
                            }

                            if (response.data.termResult.Data.TermType === 2) {
                                this.isParent = true;
                                this.selected = {
                                    Id: response.data.Data.Parent.Id,
                                    Name: response.data.Data.Parent.Name,
                                }
                                this.formTitle = "Kategori Ekle";
                                this.namePlaceholder = "Kategori Adı";
                            }
                            else if (response.data.termResult.Data.TermType === 3) {
                                this.formTitle = "Etiket Ekle";
                                this.namePlaceholder = "Etiket Adı";
                            }
                        }
                        else {
                            this.$toast({
                                component: ToastificationContent,
                                props: {
                                    variant: 'danger',
                                    title: 'Hata Oluştu!',
                                    icon: 'AlertOctagonIcon',
                                    text: response.data.termResult.Message,
                                }
                            })
                        }
                    })
                    .catch((error) => {
                        console.log(error)
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
</style>