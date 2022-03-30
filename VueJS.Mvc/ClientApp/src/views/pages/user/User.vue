<template>
    <b-row>
        <b-col class="content-header-left mb-2"
               cols="12"
               md="6">
            <b-row class="breadcrumbs-top">
                <b-col cols="12">
                    <h2 class="content-header-title float-left pr-1 mb-0">
                        Kullanıcılar
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
            <b-button v-if="$can('create', 'User')"
                      v-b-tooltip.hover
                      title="Yeni kullanıcı ekle"
                      v-ripple.400="'rgba(113, 102, 240, 0.15)'"
                      variant="primary"
                      :to="{ name:'pages-user-add'}"
                      class=" ml-1">Yeni Ekle</b-button>
        </b-col>
        <b-col md="12"
               lg="12">
            <b-card id="user-list"
                    header-tag="header"
                    no-body>
                <template #header>
                    <div class="float-left">
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
                <b-card-body v-if="isHiddenMultiDeleteButton === true && $can('delete', 'User')">
                    <div class="d-flex justify-content-between flex-wrap">
                        <b-form-group class="mb-0">
                            <b-button variant="danger"
                                      size="sm"
                                      @click="multiDeleteData">
                                <feather-icon icon="Trash2Icon"
                                              class="mr-50" />
                                <span class="align-middle">{{ checkedRowsCount }} Kullanıcıyı Sil</span>
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
                <b-table id="user-table"
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
                        <b-form-checkbox :disabled="!$can('delete', 'User') || filteredData.length <= 0"
                                         v-model="selectAllCheck"
                                         :value="true"
                                         @change="selectAllRows($event)"></b-form-checkbox>
                    </template>
                    <template #foot(Id)="slot">
                        <b-form-checkbox :disabled="!$can('delete', 'User') || filteredData.length <= 0"
                                         v-model="selectAllCheck"
                                         :value="true"
                                         @change="selectAllRows($event)"></b-form-checkbox>
                    </template>
                    <template #cell(Id)="row">
                        <b-form-checkbox :disabled="!$can('delete', 'Urlredirect')"
                                         :value="row.item.Id.toString()"
                                         :id="row.item.Id.toString()"
                                         v-model="checkedRows"
                                         @change="checkChange"></b-form-checkbox>
                    </template>
                    <template #cell(ProfileImage)="row">
                        <div v-if="row.item.ProfileImage == null"
                             class="image-icon">
                            <feather-icon icon="UserIcon"
                                          size="30" />
                        </div>
                        <div v-else
                             class="image-icon">
                            <b-img rounded
                                   v-bind:src="require('@/assets/images/media/' + row.item.ProfileImage.FileName)"
                                   :alt="row.item.ProfileImage.AltText" />
                        </div>
                    </template>
                    <template #cell(UserName)="row">
                        <b-link :to="{ name:'pages-user-edit', query: { edit : row.item.Id } }">
                            <b>{{row.item.UserName}}</b>
                        </b-link>
                        <div class="row-actions">
                            <div v-if="isHovered(row.item) && isHiddenRowActions">
                                <b-link :to=" {name: 'pages-user-view', params: { slug: row.item.UserName }}"
                                        class="text-primary small">Görüntüle</b-link>
                                <small v-if="$can('update', 'User')"
                                       class="text-muted"> | </small>
                                <b-link v-if="$can('update', 'User')"
                                        :to="{ name:'pages-user-edit', query: { edit : row.item.Id } }"
                                        class="text-success small"
                                        variant="flat-danger">Düzenle</b-link>
                                <small v-if="$can('delete', 'User')"
                                       class="text-muted"> | </small>
                                <b-link v-if="$can('delete', 'User')"
                                        href="javascript:;"
                                        no-prefetch
                                        class="text-danger small"
                                        @click="singleDeleteData(row.item.Id, row.item.UserName)">Sil</b-link>
                            </div>
                        </div>
                    </template>
                    <template #cell(Email)="row">
                        <b-link :href="'mailto:' + row.item.Email"><span> {{ row.item.Email }}</span></b-link>
                    </template>
                    <template #cell(Posts)="row">
                        <b-link :to="{ name: 'pages-basepage-list', query: { user: row.item.Id } }"><small> {{ row.item.Posts.filter(page => page.PostType === 4).length }} Temel Sayfa</small></b-link>
                        <br />
                        <b-link :to="{ name: 'pages-page-list', query: { user: row.item.Id }  }"><small> {{ row.item.Posts.filter(page => page.PostType === 0).length }} Sayfa</small></b-link>
                        <br />
                        <b-link :to="{ name: 'pages-article-list', query: { user: row.item.Id }  }"><small> {{ row.item.Posts.filter(page => page.PostType === 1).length }} Makale</small></b-link>
                    </template>
                    <template v-if="filteredData.length <= 0"
                              slot="bottom-row">
                        <td colspan="5"
                            class="text-center">
                            {{ dataNullMessage  }}
                        </td>
                    </template>
                </b-table>
                <b-card-body v-if="users.length > 0">
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
    import ToastificationContent from '@core/components/toastification/ToastificationContent.vue'
    import {
        BBreadcrumb, BBreadcrumbItem, BSpinner, BTable, BFormCheckbox, BImg, BButton, BCard, BCardBody, BRow, BCol, BFormSelect, BFormGroup, BPagination, BInputGroup, BFormInput, BInputGroupPrepend, BInputGroupAppend, VBTooltip, BLink
    } from 'bootstrap-vue'
    import Ripple from 'vue-ripple-directive'
    import axios from 'axios'

    export default {
        components: {
            ToastificationContent,
            BBreadcrumb,
            BBreadcrumbItem,
            BSpinner,
            BTable,
            BButton,
            BFormCheckbox,
            BImg,
            BCard,
            BRow,
            BCol,
            BCardBody,
            BFormSelect,
            BFormGroup,
            BPagination,
            BInputGroup,
            BFormInput,
            BInputGroupPrepend,
            BInputGroupAppend,
            BLink,
        },
        directives: {
            'b-tooltip': VBTooltip,
            Ripple,
        },
        data() {
            return {
                breadcrumbs: [
                    {
                        text: 'Kullanıcılar',
                        active: true,
                    }
                ],
                isBusy: true,
                dataNullMessage: 'Hiçbir kullanıcı bulunamadı.',
                filterText: '',
                isShowSearchTextClearButton: false,
                users: [],
                selected: '',
                selectedValue: null,
                isHiddenMultiDeleteButton: false,
                isHiddenRowActions: false,
                pageColumnQuantity: '',
                fields: [
                    { key: 'Id', sortable: false, thStyle: { width: "20px" } },
                    { key: 'ProfileImage', label: 'RESİM', sortable: false, thStyle: { width: "20px" } },
                    { key: 'UserName', label: 'Kullanıcı Adı', sortable: true, thStyle: { width: "200px" } },
                    { key: 'FirstName', label: 'Ad Soyad', sortable: true, thStyle: { width: "150px" } },
                    { key: 'Email', label: 'E-Posta Adresi', sortable: true, thStyle: { width: "100px" } },
                    { key: 'Posts', label: 'GÖNDERİLER', sortable: true, thStyle: { width: "100px" } }
                ],
                selectAllCheck: false,
                checkedRows: [],
                checkedRowsCount: '',
                hoveredRow: null,
                perPage: 10,
                pageOptions: [10, 20, 50, 100],
                totalRows: 1,
                currentPage: 1,
            }
        },
        methods: {
            getAllData() {
                this.isBusy = true;
                axios.get('/admin/user-allusers')
                    .then((response) => {
                        console.log(response.data)
                        if (response.data.Users !== null) {
                            this.users = response.data.Users;
                        } else {
                            this.users = [];
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
                                text: 'Kullanıcılar listenirken hata oluştu. Lütfen tekrar deneyiniz.',
                            }
                        })
                    });
            },
            filterByName: function (data) {
                if (this.filterText.length === 0) {
                    return true;
                }
                return (data.UserName.toLowerCase().indexOf(this.filterText.toLowerCase()) > -1);
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
                    text: name + " adlı kullanıcı kalıcı olarak silinecektir.",
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
                        axios.post('/admin/user/delete?userId=' + id)
                            .then((response) => {
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
                                    this.getAllData();
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
                    text: this.checkedRowsCount + " adet makale kalıcı olarak silinecektir?",
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
                            axios.post('/admin/user-delete?userId=' + id)
                                .then((response) => {
                                    if (response.data.PostDto.ResultStatus === 0) {
                                        this.getAllData();
                                    }
                                });
                        })
                    }
                })
            },
        },
        computed: {
            filteredData: function () {
                return this.users
                    .filter(this.filterByName);
            }
        },
        mounted() {
            this.getAllData();
        }
    }
</script>

<style>
    #user-list.card .card-header {
        padding: 8px 8px 8px 8px;
    }

    #user-table.table th, #user-table.table td {
        padding: 0.72rem 0 0.72rem 0.72rem !important;
    }

    #user-table > tbody > tr.b-table-bottom-row td {
        padding: 30px 0px 30px 0 !important;
    }

    .image-icon {
        width: 50px;
        height: 50px;
        position: relative;
    }

        .image-icon img, .image-icon svg {
            max-height: 100%;
            max-width: 100%;
            border-radius: 5px;
            position: absolute;
            top: 0;
            bottom: 0;
            left: 0;
            right: 0;
            margin: auto;
        }
</style>