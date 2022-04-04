<template>
    <b-row>
        <b-col class="content-header-left mb-2"
               cols="12"
               md="12">
            <b-row class="breadcrumbs-top">
                <b-col cols="12">
                    <h2 class="content-header-title float-left pr-1 mb-0">
                        Kategoriler
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
        <b-col v-if="$can('create', 'Category')"
               md="12"
               lg="4">
            <b-card id="card-add"
                    header-tag="header">
                <template #header>
                    <span class="font-weight-bold" style="font-size:18px;">Kategori Ekle</span>
                </template>
                <validation-observer ref="simpleRules">
                    <b-form>
                        <b-row>
                            <b-col cols="12">
                                <b-form-group label-for="name"
                                              description="Sitenizde gösterilecek olan isim.">
                                    <validation-provider #default="{ errors }"
                                                         name="name"
                                                         vid="name"
                                                         rules="required">
                                        <b-form-input id="name"
                                                      v-model="termAddDto.Name"
                                                      :state="errors.length > 0 ? false:null"
                                                      type="text"
                                                      placeholder="Kategori Adı" />
                                        <small class="text-danger">{{ errors[0] }}</small>
                                    </validation-provider>
                                </b-form-group>

                                <b-form-group label-for="slug"
                                              description="'slug' yazı isminin URL versiyonudur. Genellikle tümü küçük harflerden oluşur, sadece harf, rakam ve tire içerir.">
                                    <b-form-input id="slug"
                                                  v-model="termAddDto.Slug"
                                                  type="text"
                                                  placeholder="Kısa İsim"
                                                  @blur="changeSlug" />
                                </b-form-group>

                                <b-form-group label-for="parentTerms"
                                              description="Mevcut kategori için üst kategoriyi buradan seçebilirsiniz.">
                                    <v-select id="parentTerms"
                                              v-model="termAddDto.ParentId"
                                              :options="allParentTerms"
                                              label="Name"
                                              :reduce="(option) => option.Id"
                                              :disabled="isSelectLoading"
                          :loading="isSelectLoading"
                                              placeholder="— Üst Kategori —">
                                        <template #spinner="{ loading }">
                                            <div v-if="isSelectLoading"
                                                 style="border-left-color: rgba(88, 151, 251, 0.71)"
                                                 class="vs__spinner">
                                            </div>
                                        </template>
                                        <template #no-options="{ search, searching, loading }">
                                            {{ dataNullMessage }}
                                        </template>
                                    </v-select>
                                </b-form-group>

                                <b-form-textarea id="description"
                                                 v-model="termAddDto.Description"
                                                 placeholder="Açıklama"
                                                 rows="3" />
                                <div class="float-right mt-1">
                                    <b-spinner v-if="addButtonDisabled"
                                               variant="secondary"
                                               class="align-middle mr-1" />
                                    <b-button :disabled="addButtonDisabled"
                                              :variant="addButtonVariant"
                                              type="submit"
                                              @click.prevent="validationForm">
                                        Ekle
                                    </b-button>
                                </div>
                            </b-col>
                        </b-row>
                    </b-form>
                </validation-observer>
            </b-card>
        </b-col>
        <b-col :cols="$can('create', 'Category') ? 8 : 12">
            <b-card id="card-list"
                    header-tag="header"
                    no-body>
                <template #header>
                    <span class="font-weight-bold" style="font-size:18px;">Tüm Kategoriler </span>

                    <div v-if="terms.length > 0"
                         class="ml-auto">
                        <b-input-group size="sm"
                                       class="input-group-merge"
                                       v-on:mouseover="isShowSearchTextClearButton = true"
                                       v-on:mouseleave="isShowSearchTextClearButton = false">
                            <b-input-group-prepend is-text>
                                <feather-icon icon="SearchIcon" />
                            </b-input-group-prepend>
                            <b-form-input placeholder="Ara..."
                                          v-model="filterText"
                                          @input="allClear" />
                            <b-input-group-append is-text>
                                <feather-icon v-if="isShowSearchTextClearButton"
                                              icon="XIcon"
                                              v-b-tooltip.hover
                                              title="Temizle"
                                              class="cursor-pointer"
                                              @click="filterText = ''; allClear();" />
                            </b-input-group-append>
                        </b-input-group>
                    </div>
                    <div class="ml-auto">
                        <b-button v-b-tooltip.hover
                                  title="Tabloyu Yenile"
                                  v-ripple.400="'rgba(113, 102, 240, 0.15)'"
                                  variant="fade-secondary"
                                  class="btn-icon mr-1"
                                  size="sm"
                                  @click="getAllData()">
                            <feather-icon icon="RotateCcwIcon" />
                        </b-button>
                    </div>
                </template>
                <b-card-body v-if="isHiddenMultiDeleteButton === true && $can('delete', 'Category')">
                    <div class="d-flex justify-content-between flex-wrap">
                        <b-form-group class="mb-0">
                            <b-button variant="danger"
                                      size="sm"
                                      @click="multiDeleteData">
                                <feather-icon icon="Trash2Icon"
                                              class="mr-50" />
                                <span class="align-middle">{{ checkedRowsCount }} Kategoriyi Sil</span>
                            </b-button>
                            <b-button v-b-tooltip.hover
                                      title="Seçili kayıtları kalıcı olarak siler. Bu işlem geri alınamaz."
                                      v-ripple.400="'rgba(186, 191, 199, 0.15)'"
                                      variant="flat-secondary"
                                      size="sm"
                                      class="btn-icon rounded-circle ml-0">
                                <feather-icon icon="InfoIcon" />
                            </b-button>
                        </b-form-group>
                        <div></div>
                    </div>
                </b-card-body>
                <b-table id="category-table"
                         :busy="isBusy"
                         :items="filteredData"
                         :fields="fields"
                         :per-page="perPage"
                         :current-page="currentPage"
                         class="mb-0"
                         foot-clone
                         @row-hovered="rowHovered"
                         @row-unhovered="rowUnHovered">
                    <template #table-busy>
                        <div class="text-center text-dark my-2">
                            <b-spinner class="align-middle"></b-spinner>
                        </div>
                    </template>
                    <template #head(Id)="slot">
                        <b-form-checkbox :disabled="!$can('delete', 'Category') || filteredData.length <= 0"
                                         v-model="selectAllCheck"
                                         :value="true"
                                         @change="selectAllRows($event)"></b-form-checkbox>
                    </template>
                    <template #foot(Id)="slot">
                        <b-form-checkbox :disabled="!$can('delete', 'Category') || filteredData.length <= 0"
                                         v-model="selectAllCheck"
                                         :value="true"
                                         @change="selectAllRows($event)"></b-form-checkbox>
                    </template>
                    <template #cell(Id)="row">
                        <b-form-checkbox :disabled="!$can('delete', 'Category')"
                                         :value="row.item.Id.toString()"
                                         :id="row.item.Id.toString()"
                                         v-model="checkedRows"
                                         @change="checkChange($event)"></b-form-checkbox>
                    </template>
                    <template #cell(Name)="row">
                        <b-link v-if="$can('update', 'Category')"
                                :to="{ name:'pages-category-edit', query: { edit : row.item.Id } }">
                            <b>{{row.item.Name}}</b>
                        </b-link>
                        <b v-else>{{row.item.Name}}</b>
                        <div class="row-actions">
                            <div v-if="isHovered(row.item) && isHiddenRowActions">
                                <b-link :to=" {name: 'pages-category-view', params: { slug: row.item.Slug }}" target="_blank"
                                        class="text-primary small">Görüntüle</b-link>
                                <small v-if="$can('update', 'Category')"
                                       class="text-muted"> | </small>
                                <b-link v-if="$can('update', 'Category')"
                                        :to="{ name:'pages-category-edit', query: { edit : row.item.Id } }"
                                        class="text-success small"
                                        variant="flat-danger">Düzenle</b-link>
                                <small v-if="$can('delete', 'Category')"
                                       class="text-muted"> | </small>
                                <b-link v-if="$can('delete', 'Category')"
                                        href="javascript:;"
                                        no-prefetch
                                        class="text-danger small"
                                        @click="singleDeleteData(row.item.Id, row.item.Name)">Sil</b-link>
                            </div>
                        </div>
                    </template>
                    <template #cell(Description)="row">
                        {{ row.item.Description == '' || row.item.Description == null ? '—' : row.item.Description }}
                    </template>
                    <template #cell(PostTerms)="row">
                        <b-link :to="{ name: 'pages-article-list', query: { category: row.item.Slug } }"><small> {{ row.item.PostTerms.length }}</small></b-link>
                    </template>
                    <template v-if="filteredData.length <= 0"
                              slot="bottom-row">
                        <td colspan="5"
                            class="text-center">
                            {{ dataNullMessage  }}
                        </td>
                    </template>
                </b-table>
                <b-card-body v-if="filteredData.length > 0">
                    <div class="d-flex justify-content-between flex-wrap">

                        <!-- page length -->
                        <b-form-group label="Kayıt Sayısı: "
                                      label-cols="6"
                                      label-align="left"
                                      label-size="sm"
                                      label-for="sortBySelect"
                                      class="text-nowrap mb-md-0 mr-1">
                            <b-form-select id="perPageSelect"
                                           v-model="perPage"
                                           size="sm"
                                           inline
                                           :options="pageOptions" />
                        </b-form-group>

                        <!-- pagination -->
                        <div>
                            <b-pagination v-model="currentPage"
                                          :total-rows="totalRows"
                                          :per-page="perPage"
                                          first-number
                                          last-number
                                          prev-class="prev-item"
                                          next-class="next-item"
                                          class="mb-0"
                                          @page-click="changePage">
                                <template #prev-text>
                                    <feather-icon icon="ChevronLeftIcon"
                                                  size="18" />
                                </template>
                                <template #next-text>
                                    <feather-icon icon="ChevronRightIcon"
                                                  size="18" />
                                </template>
                            </b-pagination>
                        </div>
                        </div>
                </b-card-body>
            </b-card>
        </b-col>
    </b-row>
</template>

<script>
    import UrlHelper from '@/helper/url-helper';
    import ToastificationContent from '@core/components/toastification/ToastificationContent.vue'
    import { ValidationProvider, ValidationObserver, extend } from 'vee-validate'
    import { required } from '@validations'
    import vSelect from 'vue-select'

    import {
        BRow, BCol, BBreadcrumb, BBreadcrumbItem, BCard, BCardBody, BForm, BFormGroup, BInputGroup, BInputGroupPrepend, BInputGroupAppend, BFormInput, BFormCheckbox, BFormTextarea, BButton, VBTooltip, BSpinner, BTable, BLink, BFormSelect, BPagination,
    } from 'bootstrap-vue'
    import Ripple from 'vue-ripple-directive'
    import axios from 'axios'

    extend('required', {
        ...required,
        message: 'Lütfen gerekli bilgileri yazınız.'
    });

    export default {
        components: {
            UrlHelper,
            ToastificationContent,
            ValidationProvider,
            ValidationObserver,
            vSelect,
            BRow,
            BCol,
            BBreadcrumb,
            BBreadcrumbItem,
            BCard,
            BCardBody,
            BForm,
            BFormGroup,
            BInputGroup,
            BInputGroupPrepend,
            BInputGroupAppend,
            BFormInput,
            BFormCheckbox,
            BFormTextarea,
            BButton,
            BSpinner,
            BTable,
            BLink,
            BFormSelect,
            BPagination
        },
        directives: {
            'b-tooltip': VBTooltip,
            Ripple,
        },
        data() {
            return {
                breadcrumbs: [
                    {
                        text: 'Kategoriler',
                        active: true,
                    }
                ],
                required,
                addButtonDisabled: false,
                addButtonVariant: 'primary',
                isBusy: true,
                isSelectLoading: false,
                dataNullMessage: 'Hiçbir kategori bulunamadı.',
                filterText: '',
                isShowSearchTextClearButton: false,
                terms: [],
                allParentTerms: [],
                isHiddenMultiDeleteButton: false,
                isHiddenRowActions: false,
                pageColumnQuantity: '',
                fields: [
                    { key: 'Id', sortable: false, thStyle: { width: "20px" } },
                    { key: 'Name', label: 'İSİM', sortable: true, thStyle: { width: "200px" } },
                    { key: 'Description', label: 'Açıklama', sortable: true },
                    { key: 'Slug', label: 'KISA İSİM', sortable: true, thStyle: { width: "150px" } },
                    { key: 'PostTerms', label: 'Toplam', sortable: true, thStyle: { width: "100px" } }
                ],
                selectAllCheck: false,
                checkedRows: [],
                checkedRowsCount: 0,
                termAddDto: {
                    Name: "",
                    Slug: "",
                    ParentId: null,
                    Description: "",
                    TermType: "category"
                },
                seoObjectSettingAddDto: {
                    SeoTitle: this.Name
                },
                hoveredRow: null,
                perPage: 10,
                pageOptions: [10, 20, 50, 100],
                totalRows: 1,
                currentPage: 1,
            }
        },
        methods: {
            changeSlug() {
                var seoSlug = UrlHelper.friendlySEOUrl(this.termAddDto.Slug);
                this.termAddDto.Slug = seoSlug;
            },
            validationForm() {
                this.$refs.simpleRules.validate().then(success => {
                    if (success) {
                        this.addButtonDisabled = true;
                        this.addButtonVariant = 'outline-secondary';
                        axios.post('/admin/term-new',
                            {
                                TermAddDto: this.termAddDto,
                                SeoObjectSettingAddDto: this.seoObjectSettingAddDto
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
                                    this.getAllData();
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
                                this.addButtonDisabled = false;
                                this.addButtonVariant = 'primary';
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
            getAllData() {
                this.isBusy = true;
                this.isSelectLoading = true;
                axios.get('/admin/term-allterms', {
                    params: {
                        termType: "category"
                    }
                })
                    .then((response) => {
                        if (response.data.TermListDto.ResultStatus === 0) {
                            this.terms = response.data.TermListDto.Data.Terms;
                            this.allParentTerms = response.data.TermListDto.Data.Terms;
                            this.pageColumnQuantity = this.perPage;
                            this.totalRows = response.data.TermListDto.Data.Terms.length;
                        }
                        else {
                            this.terms = [];
                            this.allParentTerms = [];
                            this.dataNullMessage = response.data.TermListDto.Message;
                        }
                        this.isBusy = false;
                        this.isSelectLoading = false;
                        this.filterText = "";
                        this.allClear();
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
            filterByName: function (data) {
                // no search, don't filter :
                if (this.filterText.length === 0) {
                    return true;
                }

                return (data.Name.toLowerCase().indexOf(this.filterText.toLowerCase()) > -1);
            },
            allClear() {
                this.checkedRows = [];
                this.checkedRowsCount = '';
                this.isHiddenMultiDeleteButton = false;
                this.selectAllCheck = 'false';
            },
            rowHovered(item) {
                this.hoveredRow = item;
                this.isHiddenRowActions = true
            },
            isHovered(item) {
                return item == this.hoveredRow
            },
            rowUnHovered() {
                this.isHiddenRowActions = false
            },
            checkChange() {
                if (this.checkedRows.length > 0) {
                    this.isHiddenMultiDeleteButton = true;
                    this.checkedRowsCount = this.checkedRows.length > 0 ? "( " + this.checkedRows.length + " )" : '';

                    var pageDataQuantity = this.filteredData.length - (this.pageColumnQuantity - this.perPage);
                    if (this.pageColumnQuantity > this.perPage) {
                        if (this.checkedRows.length === this.perPage) {
                            this.selectAllCheck = 'true';
                        }
                        else if (this.checkedRows.length < this.perPage && pageDataQuantity > 0 && this.checkedRows.length === pageDataQuantity) {
                            this.selectAllCheck = 'true';
                        }
                        else {
                            this.selectAllCheck = 'false';
                        }
                    } else if (this.pageColumnQuantity === this.perPage) {
                        if (this.checkedRows.length === this.perPage) {
                            this.selectAllCheck = 'true';
                        }
                        else if (this.checkedRows.length < this.perPage && this.filteredData.length === this.checkedRows.length) {
                            this.selectAllCheck = 'true';
                        }
                        else {
                            this.selectAllCheck = 'false';
                        }
                    }
                }
                else {
                    this.isHiddenMultiDeleteButton = false;
                    this.selectAllCheck = 'false';
                }
            },
            selectAllRows(value) {
                if (value === true) {
                    var idList = [];
                    if (this.pageColumnQuantity > this.perPage) {
                        for (var i = this.pageColumnQuantity - this.perPage; i < this.pageColumnQuantity; i++) {
                            if (this.filteredData[i] != null) {
                                idList.push(this.filteredData[i].Id);
                            }
                        }
                    } else if (this.pageColumnQuantity === this.perPage) {
                        for (var i = 0; i < this.pageColumnQuantity; i++) {
                            if (this.filteredData[i] != null) {
                                idList.push(this.filteredData[i].Id);
                            }
                        }
                    } else if (this.perPage > this.pageColumnQuantity) {
                        for (var i = 0; i < this.perPage; i++) {
                            if (this.filteredData[i] != undefined) {
                                idList.push(this.filteredData[i].Id);
                            }
                        }
                    }
                    this.checkedRows = idList;
                }
                else {
                    this.checkedRows = [];
                    this.isHiddenMultiDeleteButton = false;
                }
                this.checkChange();
            },
            changePage(e, pageNumber) {
                this.checkedRows = [];
                this.checkChange();
                this.pageColumnQuantity = this.perPage * pageNumber;
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
                        axios.post('/admin/term-delete?termId=' + id)
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
            multiDeleteData() {
                this.$swal({
                    title: 'Toplu olarak silmek istediğinizden emin misiniz?',
                    text: this.checkedRowsCount + " kategori kalıcı olarak silinecektir?",
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
                        this.checkedRows.forEach((id, index) => {
                            axios.post('/admin/term-delete?termId=' + id)
                                .then((response) => {
                                    if (response.data.ResultStatus === 0) {
                                        this.checkedRowsCount = "";
                                        this.isHiddenMultiDeleteButton = false;
                                        this.getAllData();
                                    }
                                });
                        });
                    }
                })
            },
            
        },
        computed: {
            filteredData: function () {
                return this.terms
                    .filter(this.filterByName);
            }
        },
        mounted() {
            this.getAllData();
        }
    }
</script>

<style lang="scss">
    @import '@core/scss/vue/libs/vue-select.scss';

    #card-add.card .card-header {
        padding: 8px 8px 8px 20px !important;
        border-bottom: 1px solid #ebe9f1;
        margin-bottom: 20px;
    }

    #card-list.card .card-header {
        padding: 8px 8px 8px 8px !important;
    }

    #category-table.table th, #category-table.table td {
        padding: 0.72rem !important;
    }

    #category-table > tbody > tr.b-table-bottom-row td {
        padding: 30px 0px 30px 0 !important;
    }

    #category-table.table th:last-child, #category-table.table td:last-child {
        text-align: center;
    }
</style>