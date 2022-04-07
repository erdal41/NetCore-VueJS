<template>
    <b-row>
        <b-col cols="12">
            <b-row class="content-header">
                <b-col class="content-header-left mb-2"
                       cols="12"
                       md="8">
                    <h2 class="content-header-title float-left pr-1 mb-0">
                        Bileşen Ayarları
                    </h2>
                    <div class="breadcrumb-wrapper">
                        <b-breadcrumb>
                            <b-breadcrumb-item to="/">
                                <feather-icon icon="HomeIcon"
                                              size="16"
                                              class="align-text-top" />
                            </b-breadcrumb-item>
                            <b-breadcrumb-item active>Bileşen Ayarlarını Düzenle</b-breadcrumb-item>
                        </b-breadcrumb>
                    </div>
                </b-col>
                <b-col class="content-header-right text-md-right d-md-block d-none mb-1"
                       md="4"
                       cols="12">
                    <b-spinner v-if="buttonDisabled"
                               variant="secondary"
                               class="align-middle mr-1" />
                    <b-button v-if="$can('update', 'Seo')"
                              :disabled="buttonDisabled"
                              :variant="updateButtonVariant"
                              type="button"
                              @click.prevent="updateData">
                        Güncelle
                    </b-button>
                </b-col>
            </b-row>
            <b-overlay :show="showOverlay"
                       rounded="sm"
                       opacity="0.80"
                       variant="white"
                       no-fade>
                <b-row>
                    <b-col cols="12">
                                <b-card id="card-email"
                            header-tag="header">
                                    <template #header>
                                        <span class="font-weight-bold" style="font-size:18px;">Bileşen Ayarları</span>
                                    </template>
                                    <b-row>
                                        <b-col lg="3"
                                               md="3"
                                               sm="6">
                                            <b-form-group label="Bileşen Başlığı"
                                                          label-for="header">
                                                <b-form-input id="header"
                                                              v-model="articleRightSideBarWidgetOptionsDto.Header"
                                                              placeholder="Bileşen başlığını giriniz" />
                                            </b-form-group>
                                        </b-col>
                                        <b-col lg="3"
                                               md="3"
                                               sm="6">
                                            <b-form-group label="Gösterilecek Makale Sayısı"
                                                          label-for="takeSize">
                                                <b-form-spinbutton id="takeSize"
                                                                   v-model="articleRightSideBarWidgetOptionsDto.TakeSize"
                                                                   min="0"
                                                                   max="50" />
                                            </b-form-group>
                                        </b-col>
                                        <b-col lg="3"
                                               md="3"
                                               sm="6">
                                            <b-form-group label="Kategori"
                                                          label-for="category">
                                                <v-select id="category"
                                                          v-model="articleRightSideBarWidgetOptionsDto.CategoryId"
                                                          :options="categories"
                                                          label="Name"
                                                          :reduce="(option) => option.Id"
                                                          :clearable="false"
                                                          placeholder="— Seçim Yapın —" />
                                            </b-form-group>
                                        </b-col>
                                        <b-col lg="3"
                                               md="3"
                                               sm="6">
                                            <b-form-group label="Etiket"
                                                          label-for="tag">
                                                <v-select id="tag"
                                                          v-model="articleRightSideBarWidgetOptionsDto.TagId"
                                                          :options="tags"
                                                          label="Name"
                                                          :reduce="(option) => option.Id"
                                                          :clearable="false"
                                                          placeholder="— Seçim Yapın —" />
                                            </b-form-group>
                                        </b-col>
                                        <b-col lg="3"
                                               md="3"
                                               sm="6">
                                            <b-form-group label="Filtre Türü"
                                                          label-for="filter">
                                                <v-select id="filter"
                                                          v-model="articleRightSideBarWidgetOptionsDto.FilterBy"
                                                          :value="articleRightSideBarWidgetOptionsDto.FilterBy"
                                                          :options="filters"
                                                          label="Name"
                                                          :reduce="(option) => option.Id"
                                                          :clearable="false"
                                                          placeholder="— Seçim Yapın —" />
                                            </b-form-group>
                                        </b-col>
                                        <b-col lg="3"
                                               md="3"
                                               sm="6">
                                            <b-form-group label="Sıralama Türü"
                                                          label-for="order">
                                                <v-select id="order"
                                                          v-model="articleRightSideBarWidgetOptionsDto.OrderBy"
                                                          :value="articleRightSideBarWidgetOptionsDto.OrderBy"
                                                          :options="orders"
                                                          label="Name"
                                                          :reduce="(option) => option.Id"
                                                          :clearable="false"
                                                          placeholder="— Seçim Yapın —" />
                                            </b-form-group>
                                        </b-col>
                                        <b-col lg="3"
                                               md="3"
                                               sm="6">
                                            <b-form-group label="Başlangıç Tarihi"
                                                          label-for="startAt">
                                                <b-form-datepicker id="startAt"
                                                                   v-model="articleRightSideBarWidgetOptionsDto.StartAt">
                                                </b-form-datepicker>
                                            </b-form-group>
                                        </b-col>
                                        <b-col lg="3"
                                               md="3"
                                               sm="6">
                                            <b-form-group label="Bitiş Tarihi"
                                                          label-for="endAt">
                                                <b-form-datepicker id="endAt"
                                                                   v-model="articleRightSideBarWidgetOptionsDto.EndAt">
                                                </b-form-datepicker>
                                            </b-form-group>
                                        </b-col>
                                        <b-col lg="3"
                                               md="3"
                                               sm="6">
                                            <b-form-group label="Minimum Okunma Sayısı"
                                                          label-for="minViewCount">
                                                <b-form-spinbutton id="minViewCount"
                                                                   v-model="articleRightSideBarWidgetOptionsDto.MinViewCount"
                                                                   min="0"
                                                                   max="1000000000000000000" />
                                            </b-form-group>
                                        </b-col>
                                        <b-col lg="3"
                                               md="3"
                                               sm="6">
                                            <b-form-group label="Maksimum Okunma Sayısı"
                                                          label-for="maxViewCount">
                                                <b-form-spinbutton id="maxViewCount"
                                                                   v-model="articleRightSideBarWidgetOptionsDto.MaxViewCount"
                                                                   min="0"
                                                                   max="1000000000000000000" />
                                            </b-form-group>
                                        </b-col>
                                        <b-col lg="3"
                                               md="3"
                                               sm="6">
                                            <b-form-group label="Minimum Yorum Sayısı"
                                                          label-for="minCommentCount">
                                                <b-form-spinbutton id="minCommentCount"
                                                                   v-model="articleRightSideBarWidgetOptionsDto.MinCommentCount"
                                                                   min="0"
                                                                   max="1000000000000000000" />
                                            </b-form-group>
                                        </b-col>
                                        <b-col lg="3"
                                               md="3"
                                               sm="6">
                                            <b-form-group label="Maksimum Yorum Sayısı"
                                                          label-for="maxCommentCount">
                                                <b-form-spinbutton id="maxCommentCount"
                                                                   v-model="articleRightSideBarWidgetOptionsDto.MaxCommentCount"
                                                                   min="0"
                                                                   max="1000000000000000000" />
                                            </b-form-group>
                                        </b-col>
                                    </b-row>
                                </b-card>
                    </b-col>
                </b-row>
            </b-overlay>
        </b-col>
    </b-row>
</template>

<script>
    import vSelect from 'vue-select';
    import ToastificationContent from '@core/components/toastification/ToastificationContent.vue';
    import { BRow, BCol, BBreadcrumb, BBreadcrumbItem, BSpinner, BButton, BOverlay, BCard, BFormGroup, BFormInput, BFormDatepicker, BFormSpinbutton } from 'bootstrap-vue';
    import axios from 'axios';

    export default {
        components: {
            vSelect,
            ToastificationContent,
            BRow,
            BCol,
            BBreadcrumb,
            BBreadcrumbItem,
            BSpinner,
            BButton,
            BOverlay,
            BCard,
            BFormGroup,
            BFormInput,
            BFormDatepicker,
            BFormSpinbutton
        },
        data() {
            return {
                buttonDisabled: false,
                updateButtonVariant: 'primary',
                showOverlay: true,
                articleRightSideBarWidgetOptionsDto: {
                    Header: '',
                    TakeSize: '',
                    CategoryId: '',
                    TagId: '',
                    FilterBy: '',
                    OrderBy: '',
                    IsAscending: false,
                    StartAt: '',
                    EndAt: '',
                    MinViewCount: '',
                    MaxViewCount: '',
                    MinCommentCount: '',
                    MaxCommentCount: '',
                },
                categories: [],
                tags: [],
                filters: [],
                orders: []
            }
        },
        methods: {
            getData() {
                this.showOverlay = true;
                axios.get('/admin/settings-widget')
                    .then((response) => {
                        if (response.data.ArticleRightSideBarWidgetOptionsDto != null) {
                            this.articleRightSideBarWidgetOptionsDto.Header = response.data.ArticleRightSideBarWidgetOptionsDto.Header;
                            this.articleRightSideBarWidgetOptionsDto.TakeSize = response.data.ArticleRightSideBarWidgetOptionsDto.TakeSize;
                            this.articleRightSideBarWidgetOptionsDto.CategoryId = response.data.ArticleRightSideBarWidgetOptionsDto.CategoryId;
                            this.articleRightSideBarWidgetOptionsDto.TagId = response.data.ArticleRightSideBarWidgetOptionsDto.TagId;
                            this.articleRightSideBarWidgetOptionsDto.FilterBy = response.data.ArticleRightSideBarWidgetOptionsDto.FilterBy;
                            this.articleRightSideBarWidgetOptionsDto.OrderBy = response.data.ArticleRightSideBarWidgetOptionsDto.OrderBy;
                            this.articleRightSideBarWidgetOptionsDto.IsAscending = response.data.ArticleRightSideBarWidgetOptionsDto.IsAscending;
                            this.articleRightSideBarWidgetOptionsDto.StartAt = response.data.ArticleRightSideBarWidgetOptionsDto.StartAt;
                            this.articleRightSideBarWidgetOptionsDto.EndAt = response.data.ArticleRightSideBarWidgetOptionsDto.EndAt;
                            this.articleRightSideBarWidgetOptionsDto.MinViewCount = response.data.ArticleRightSideBarWidgetOptionsDto.MinViewCount;
                            this.articleRightSideBarWidgetOptionsDto.MaxViewCount = response.data.ArticleRightSideBarWidgetOptionsDto.MaxViewCount;
                            this.articleRightSideBarWidgetOptionsDto.MinCommentCount = response.data.ArticleRightSideBarWidgetOptionsDto.MinCommentCount;
                            this.articleRightSideBarWidgetOptionsDto.MaxCommentCount = response.data.ArticleRightSideBarWidgetOptionsDto.MaxCommentCount;

                            this.categories = response.data.ArticleRightSideBarWidgetOptionsDto.Categories;
                            this.tags = response.data.ArticleRightSideBarWidgetOptionsDto.Tags;
                        }
                        this.showOverlay = false;
                    })
                    .catch((error) => {
                        this.$toast({
                            component: ToastificationContent,
                            props: {
                                variant: 'danger',
                                title: 'Hata Oluştu!',
                                icon: 'AlertOctagonIcon',
                                text: 'Form ayarları yüklenirken hata oluştu. Lütfen tekrar deneyin.',
                            }
                        })
                    });

            },
            getFilterBy() {
                axios.get('/admin/settings-getfilter')
                    .then((response) => {
                        this.filters = response.data;
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
            getOrderBy() {
                axios.get('/admin/settings-getorder')
                    .then((response) => {
                        this.orders = response.data;
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
            updateData() {
                this.buttonDisabled = true;
                this.updateButtonVariant = 'outline-secondary';
                axios.post('/admin/settings-widget', {
                    ArticleRightSideBarWidgetOptionsDto: this.articleRightSideBarWidgetOptionsDto
                })
                    .then((response) => {
                        if (response.data.ArticleRightSideBarWidgetOptions != null) {
                            this.$toast({
                                component: ToastificationContent,
                                props: {
                                    variant: 'success',
                                    title: 'Başarılı İşlem!',
                                    icon: 'CheckIcon',
                                    text: 'Form ayarları güncellendi.'
                                }
                            });
                        }
                        this.buttonDisabled = false;
                        this.updateButtonVariant = 'primary';
                    })
                    .catch((error) => {
                        this.$toast({
                            component: ToastificationContent,
                            props: {
                                variant: 'danger',
                                title: 'Hata Oluştu!',
                                icon: 'AlertOctagonIcon',
                                text: 'Form ayarları yüklenirken hata oluştu. Lütfen tekrar deneyin.',
                            }
                        })
                    });
            }
        },
        mounted() {
            this.getData();
            this.getFilterBy();
            this.getOrderBy();
        }
    }
</script>

<style lang="scss">
    @import '@core/scss/vue/libs/vue-select.scss';

    #card-email .card-header {
        padding: 8px 8px 8px 20px;
        border-bottom: 1px solid #ebe9f1;
        margin-bottom: 20px;
    }
</style>