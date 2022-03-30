<template>
    <b-row>
        <modal-edit :urlredirect-id="urlRedirectId"
                    @getAllData="getAllData"></modal-edit>
        <b-col class="content-header-left mb-2"
               cols="12"
               md="12">
            <b-row class="breadcrumbs-top">
                <b-col cols="12">
                    <h2 class="content-header-title float-left pr-1 mb-0">
                        Link Yönlendirmeleri
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
        <b-col v-if="$can('create', 'Urlredirect')"
               md="12"
               lg="12">
                <b-card id="card-add"
                        header-tag="header">
                    <template #header>
                        <span class="font-weight-bold" style="font-size:18px;">Yönlendirme Ekle</span>
                    </template>
                    <validation-observer ref="simpleRules">
                            <b-row>
                                <b-col cols="12">
                                    <b-form-group label-for="oldurl"
                                                  description="Yönlendirilecek linki giriniz.">
                                        <validation-provider #default="{ errors }"
                                                             name="oldurl"
                                                             rules="required">
                                            <b-form-input id="oldurl"
                                                          v-model="urlRedirectAddDto.OldUrl"
                                                          :state="errors.length > 0 ? false:null"
                                                          type="text"
                                                          placeholder="Eski Url" />
                                            <small class="text-danger">{{ errors[0] }}</small>
                                        </validation-provider>
                                    </b-form-group>

                                    <b-form-group label-for="newurl"
                                                  description="Yönelecek olan linki giriniz.">
                                        <validation-provider #default="{ errors }"
                                                             name="newurl"
                                                             rules="required">
                                            <b-form-input id="newurl"
                                                          v-model="urlRedirectAddDto.NewUrl"
                                                          :state="errors.length > 0 ? false:null"
                                                          type="text"
                                                          placeholder="Yeni Url" />
                                            <small class="text-danger">{{ errors[0] }}</small>
                                        </validation-provider>
                                    </b-form-group>

                                    <b-form-textarea id="description"
                                                     v-model="urlRedirectAddDto.Description"
                                                     placeholder="Açıklama"
                                                     rows="2" />

                                    <!-- reset button -->
                                    <div class="float-right mt-1">
                                        <b-spinner v-if="addButtonDisabled"
                                                   variant="secondary"
                                                   class="align-middle mr-1"/>
                                        <b-button :disabled="addButtonDisabled"
                                                  :variant="addButtonVariant"                                                  
                                                  @click.prevent="validationForm">
                                            Ekle
                                        </b-button>
                                    </div>
                                </b-col>
                            </b-row>
                    </validation-observer>
                </b-card>
                </b-col>
        <b-col md="12"
               lg="12">
            <b-card id="url-redirect-list"
                    header-tag="header"
                    no-body>
                <template #header>
                    <span class="font-weight-bold" style="font-size:18px;">Tüm Yönlendirmeler</span>
                    <div v-if="urlRedirects.length > 0"
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
                                          @input="allClear"/>
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
                <b-card-body v-if="isHiddenMultiDeleteButton === true && $can('delete', 'Urlredirect')">
                    <div class="d-flex justify-content-between flex-wrap">
                        <b-form-group class="mb-0">
                            <b-button variant="danger"
                                      size="sm"
                                      @click="multiDeleteData">
                                <feather-icon icon="Trash2Icon"
                                              class="mr-50" />
                                <span class="align-middle">{{ checkedRowsCount }} Yönlendirmeyi Sil</span>
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
                    </div>
                </b-card-body>
                <b-table id="urlredirect-table"
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
                        <b-form-checkbox :disabled="!$can('delete', 'Urlredirect') || filteredData.length <= 0"
                                         v-model="selectAllCheck"
                                         :value="true"
                                         @change="selectAllRows($event)"></b-form-checkbox>
                    </template>
                    <template #foot(Id)="slot">
                        <b-form-checkbox :disabled="!$can('delete', 'Urlredirect') || filteredData.length <= 0"
                                         v-model="selectAllCheck"
                                         :value="true"
                                         @change="selectAllRows($event)"></b-form-checkbox>
                    </template>
                    <template #cell(Id)="row">
                        <b-form-checkbox :disabled="!$can('delete', 'Urlredirect')"
                                         :value="row.item.Id.toString()"
                                         :id="row.item.Id.toString()"
                                         v-model="checkedRows"
                                         @change="checkChange($event)"></b-form-checkbox>
                    </template>
                    <template #cell(OldUrl)="row">
                        <b-link :href="row.item.OldUrl"
                                target="_blank">
                            <b>
                                {{row.item.OldUrl}}
                                <feather-icon icon="ExternalLinkIcon"
                                              size="14" />
                            </b><br />
                            <span class="text-muted">{{row.item.NewUrl}}</span>
                        </b-link>
                        <div class="row-actions">
                            <div v-if="isHovered(row.item) && isHiddenRowActions">
                                <b-link :href="row.item.OldUrl"
                                        class="text-primary small">Görüntüle</b-link>
                                <small v-if="$can('update', 'Urlredirect')"
                                       class="text-muted"> | </small>
                                <b-link v-if="$can('update', 'Urlredirect')"
                                        v-b-modal.modal-edit
                                        class="text-success small"
                                        variant="flat-danger"
                                        @click.prevent=updateClick(row.item.Id)>Düzenle</b-link>
                                <small v-if="$can('delete', 'Urlredirect')"
                                       class="text-muted"> | </small>
                                <b-link v-if="$can('delete', 'Urlredirect')"
                                        href="javascript:;"
                                        no-prefetch
                                        class="text-danger small"
                                        @click="singleDeleteData(row.item.Id, row.item.OldUrl)">Sil</b-link>
                            </div>
                        </div>
                    </template>
                    <template #cell(ModifiedDate)="row">
                        <span>{{ new Date(row.item.ModifiedDate).toLocaleString() }}</span>
                    </template>
                    <template #cell(User.UserName)="row">
                        <b-link :to="{ name: 'pages-user-edit', query: { edit: row.item.User.Id } }">
                            <span class="font-weight-bold">{{ row.item.User.UserName }}</span>
                        </b-link>
                    </template>
                    <template v-if="filteredData.length <= 0"
                              slot="bottom-row">
                        <td colspan="5"
                            class="text-center">
                            {{ dataNullMessage  }}
                        </td>
                    </template>
                </b-table>
                <b-card-body v-if="urlRedirects.length > 0">
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
                </b-card-body>
            </b-card>
        </b-col>
    </b-row>
</template>

<script>
    import ModalEdit from './ModalEdit.vue';
    import moment from 'moment';
    import { ValidationProvider, ValidationObserver, extend } from 'vee-validate'
    import { required } from '@validations'
    import ToastificationContent from '@core/components/toastification/ToastificationContent.vue'
    import Ripple from 'vue-ripple-directive'
    import {
        BRow, BCol, BBreadcrumb, BBreadcrumbItem, BCard, BCardBody, BForm, BFormGroup, BInputGroup, BInputGroupPrepend, BInputGroupAppend, BFormInput, BFormCheckbox, BFormTextarea, BButton, VBTooltip, BSpinner, BTable, BLink, BFormSelect, BPagination,
    } from 'bootstrap-vue'
    import axios from 'axios'


    extend('required', {
        ...required,
        message: 'Lütfen gerekli bilgileri yazınız.'
    });

    export default {
        components: {
            ModalEdit,
            moment,
            ValidationProvider,
            ValidationObserver,
            ToastificationContent,
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
                        text: 'Link Yönlendirmeleri',
                        active: true,
                    }
                ],
                required,
                addButtonDisabled: false,
                addButtonVariant: 'primary',
                urlRedirectAddDto: {
                    OldUrl: '',
                    NewUrl: '',
                    Description: ''
                },
                isBusy: true,
                dataNullMessage: 'Hiçbir yönlendirme bulunamadı.',
                filterText: '',
                isShowSearchTextClearButton: false,
                urlRedirects: [],
                isHiddenMultiDeleteButton: false,
                isHiddenRowActions: false,
                pageColumnQuantity: '',
                fields: [
                    { key: 'Id', sortable: false, thStyle: { width: "20px" } },
                    { key: 'OldUrl', label: 'Url', sortable: true, thStyle: { width: "200px" } },
                    { key: 'Description', label: 'Açıklama', sortable: true, thStyle: { width: "200px" } },
                    { key: 'ModifiedDate', label: 'Tarih', sortable: true, thStyle: { width: "150px" } },
                    { key: 'User.UserName', label: 'İşlem Yapan', sortable: true, thStyle: { width: "100px" } }
                ],
                selectAllCheck: false,
                checkedRows: [],
                checkedRowsCount: '',
                hoveredRow: null,
                perPage: 10,
                pageOptions: [10, 20, 50, 100],
                totalRows: 1,
                currentPage: 1,
                urlRedirectId: 0
            }
        },
        methods: {
            validationForm() {
                this.$refs.simpleRules.validate().then(success => {
                    if (success) {
                        if (!this.urlRedirectAddDto.OldUrl.includes('https://') && !this.urlRedirectAddDto.OldUrl.includes('http://')) {
                            this.$toast({
                                component: ToastificationContent,
                                props: {
                                    variant: 'warning',
                                    title: 'Uyarı!',
                                    icon: 'AlertTriangleIcon',
                                    text: 'Eski url, "https://" veya "http://" parametresi ile beraber tam linki içermerlidir. Örnek: https://www.orneksite.com/ornek-sayfa'
                                }
                            });
                        } else if (!this.urlRedirectAddDto.NewUrl.includes('https://') && !this.urlRedirectAddDto.NewUrl.includes('http://')) {
                            this.$toast({
                                component: ToastificationContent,
                                props: {
                                    variant: 'warning',
                                    title: 'Uyarı!',
                                    icon: 'AlertTriangleIcon',
                                    text: 'Yeni url, "https://" veya "http://" parametresi ile beraber tam linki içermerlidir. Örnek: https://www.orneksite.com/ornek-sayfa'
                                }
                            });
                        } else {
                            this.addButtonDisabled = true;
                            this.addButtonVariant = 'outline-secondary';
                            axios.post('/admin/urlredirect-new',
                                {
                                    UrlRedirectAddDto: this.urlRedirectAddDto
                                })
                                .then((response) => {
                                    if (response.data.UrlRedirectDto.ResultStatus === 0) {
                                        this.$toast({
                                            component: ToastificationContent,
                                            props: {
                                                variant: 'success',
                                                title: 'Başarılı İşlem!',
                                                icon: 'CheckIcon',
                                                text: this.urlRedirectAddDto.OldUrl + " linki " + this.urlRedirectAddDto.NewUrl + " linkine yönlendirildi."
                                            }
                                        });
                                        this.getAllData();
                                        this.addButtonDisabled = false;
                                        this.addButtonVariant = 'primary';
                                    }
                                    else {
                                        this.$toast({
                                            component: ToastificationContent,
                                            props: {
                                                variant: 'danger',
                                                title: 'Başarısız İşlem!',
                                                icon: 'AlertOctagonIcon',
                                                text: response.data.UrlRedirectDto.Message
                                            },
                                        });
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
                    }
                })
            },
            getAllData() {
                this.isBusy = true;
                axios.get('/admin/urlredirect-allurlredirects')
                    .then((response) => {
                        if (response.data.UrlRedirectListDto.ResultStatus === 0) {
                            this.urlRedirects = response.data.UrlRedirectListDto.Data.UrlRedirects;
                            this.pageColumnQuantity = this.perPage;
                            this.totalRows = response.data.UrlRedirectListDto.Data.UrlRedirects.length;
                        } else {
                            this.urlRedirects = [];
                            this.dataNullMessage = response.data.UrlRedirectListDto.Message;
                        }

                        this.isBusy = false;
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
                                text: 'Yönlendirmeler listenirken hata oluştu. Lütfen tekrar deneyiniz.',
                            }
                        })
                    });
            },
            filterByName: function (data) {
                // no search, don't filter :
                if (this.filterText.length === 0) {
                    return true;
                }
                return (data.OldUrl.toLowerCase().indexOf(this.filterText.toLowerCase()) > -1);
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
            updateClick(id) {
                this.urlRedirectId = id;
            },
            singleDeleteData(id, name) {
                this.$swal({
                    title: 'Silmek istediğinize emin misiniz?',
                    text: name + " adlı link kalıcı olarak silinecektir?",
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
                        axios.post('/admin/urlredirect-delete?urlRedirectId=' + id)
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
                    text: this.checkedRowsCount + " yönlendirme kalıcı olarak silinecektir?",
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
                            axios.post('/admin/urlredirect-delete?urlRedirectId=' + id)
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
                return this.urlRedirects
                    .filter(this.filterByName);
            },
        },
        mounted() {
            this.getAllData();
        }
    }
</script>

<style lang="scss">
    #card-add.card .card-header{
        padding: 8px 8px 8px 20px;
        border-bottom: 1px solid #ebe9f1;
        margin-bottom: 20px;
    }

    #url-redirect-list.card .card-header {
        padding: 8px 8px 8px 20px;
    }

    #urlredirect-table.table th, #urlredirect-table.table td {
        padding: 0.72rem !important;
    }

    #urlredirect-table > tbody > tr.b-table-bottom-row td {
        padding: 30px 0px 30px 0 !important;
    }
</style>