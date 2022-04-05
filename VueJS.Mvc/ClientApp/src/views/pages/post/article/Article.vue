<template>
    <b-row>
        <b-col class="content-header-left mb-2"
               cols="12"
               md="6">
            <b-row class="breadcrumbs-top">
                <b-col cols="12">
                    <h2 class="content-header-title float-left pr-1 mb-0">
                        Makaleler
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
            <b-button v-if="$can('create', 'Article')"
                      v-b-tooltip.hover
                      title="Yeni makale ekle"
                      v-ripple.400="'rgba(113, 102, 240, 0.15)'"
                      variant="primary"
                      :to="{ name:'pages-article-add'}"
                      class=" ml-1">Yeni Ekle</b-button>
        </b-col>
        <b-col md="12"
               lg="12">
            <b-card id="article-list"
                    header-tag="header"
                    no-body>
                <template #header>
                    <div v-if="posts.length > 0"
                         class="float-left">
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
                                <feather-icon v-show="isShowSearchTextClearButton"
                                              icon="XIcon"
                                              v-b-tooltip.hover
                                              title="Temizle"
                                              class="cursor-pointer"
                                              @click="filterText = ''; allClear();" />
                            </b-input-group-append>
                        </b-input-group>
                    </div>
                    <div class="ml-auto">
                        <b-link v-b-tooltip.hover
                                title="Yayında ve taslak olan makaleler"
                                v-ripple.400="'rgba(113, 102, 240, 0.15)'"
                                variant="fade-secondary"
                                :to="{ name:'pages-article-list'}"
                                class="text-primary small mr-1">Tümü ( {{ publishPostsCount + draftPostsCount  }} )</b-link>
                        <b-link v-show="publishPostsCount > 0"
                                v-b-tooltip.hover
                                title="Yayında olan makaleler"
                                v-ripple.400="'rgba(113, 102, 240, 0.15)'"
                                variant="fade-secondary"
                                :to="{ name:'pages-article-list', query: { status : 'publish' } }"
                                class="text-primary small mr-1">Yayında ( {{ publishPostsCount }} )</b-link>
                        <b-link v-show="draftPostsCount > 0"
                                v-b-tooltip.hover
                                title="Taslak olan makaleler"
                                v-ripple.400="'rgba(113, 102, 240, 0.15)'"
                                variant="fade-secondary"
                                :to="{ name:'pages-article-list', query: { status : 'draft' } }"
                                class="text-warning small mr-1">Taslak ( {{ draftPostsCount }} )</b-link>
                        <b-link v-show="trashPostsCount > 0"
                                v-b-tooltip.hover
                                title="Çöp kutusunda olan makaleler"
                                v-ripple.400="'rgba(113, 102, 240, 0.15)'"
                                variant="fade-secondary"
                                :to="{ name:'pages-article-list', query: { status : 'trash' } }"
                                class="text-danger small mr-1">Çöp ( {{ trashPostsCount }} )</b-link>
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
                <b-card-body v-show="isHiddenStatusButton === true">
                    <div class="d-flex justify-content-between flex-wrap">
                        <b-form-group class="mb-0">
                            <b-dropdown id="dropdown-left"
                                        text="Durum"
                                        variant="link"
                                        no-caret
                                        toggle-class="p-0"
                                        left>

                                <template #button-content>
                                    <b-button v-ripple.400="'rgba(255, 255, 255, 0.15)'"
                                              variant="primary"
                                              class="btn-icon">
                                        Durum
                                        <feather-icon icon="ChevronDownIcon" />
                                        <span>{{ checkedRowsCount }}</span>
                                    </b-button>
                                    <b-button v-b-tooltip.hover
                                              title="Seçili makelelerin durumlarını değiştirmenizi sağlar."
                                              v-ripple.400="'rgba(186, 191, 199, 0.15)'"
                                              variant="flat-secondary"
                                              size="sm"
                                              class="btn-icon rounded-circle ml-0">
                                        <feather-icon icon="InfoIcon" />
                                    </b-button>
                                </template>
                                <b-dropdown-item v-if="$can('update', 'Article') && $route.query.status !== 'publish' && $route.query.status !== 'trash'"
                                                 href="javascript:;"
                                                 id="multi-publish"
                                                 variant="success"
                                                 @click="multiPostStatusChange">
                                    Yayınla
                                </b-dropdown-item>
                                <b-dropdown-item v-if="$can('update', 'Article') && $route.query.status === 'publish'"
                                                 href="javascript:;"
                                                 id="multi-draft"
                                                 variant="warning"
                                                 @click="multiPostStatusChange">
                                    Taslak Olarak Kaydet
                                </b-dropdown-item>
                                <b-dropdown-item v-if="$can('update', 'Article') && $route.query.status !== 'trash'"
                                                 href="javascript:;"
                                                 id="multi-trash"
                                                 variant="danger"
                                                 @click="multiPostStatusChange">
                                    Çöp Kutusuna Taşı
                                </b-dropdown-item>
                                <b-dropdown-item v-if="$can('update', 'Article') && $route.query.status === 'trash'"
                                                 href="javascript:;"
                                                 id="multi-untrash"
                                                 variant="warning"
                                                 @click="multiPostStatusChange">
                                    Geri Al
                                </b-dropdown-item>
                                <b-dropdown-item v-if="$can('delete', 'Article') && $route.query.status === 'trash'"
                                                 href="javascript:;"
                                                 id="multi-delete"
                                                 variant="danger"
                                                 @click="multiDeleteData">
                                    Kalıcı Olarak Sil
                                </b-dropdown-item>
                            </b-dropdown>                            
                        </b-form-group>
                    </div>
                </b-card-body>
                <b-table id="article-table"
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
                        <b-form-checkbox :disabled="((!$can('update', 'Article') && $route.query.status !== 'trash') || (!$can('delete', 'Article') && $route.query.status === 'trash')) || filteredData.length <= 0"
                                         v-model="selectAllCheck"
                                         :value="true"
                                         @change="selectAllRows($event)"></b-form-checkbox>
                    </template>
                    <template #foot(Id)="slot">
                        <b-form-checkbox :disabled="((!$can('update', 'Article') && $route.query.status !== 'trash') || (!$can('delete', 'Article') && $route.query.status === 'trash')) || filteredData.length <= 0"
                                         v-model="selectAllCheck"
                                         :value="true"
                                         @change="selectAllRows($event)"></b-form-checkbox>
                    </template>
                    <template #cell(Id)="row">
                        <b-form-checkbox :disabled="((!$can('update', 'Article') && $route.query.status !== 'trash') || (!$can('delete', 'Article') && $route.query.status === 'trash'))"
                                         :value="row.item.Id.toString()"
                                         :id="row.item.Id.toString()"
                                         v-model="checkedRows"
                                         @change="checkChange($event)"></b-form-checkbox>
                    </template>
                    <template #cell(FeaturedImage)="row">
                        <div class="image-icon">
                            <b-img rounded
                                   v-bind:src="row.item.FeaturedImage == null ? noImage : require('@/assets/images/media/' + row.item.FeaturedImage.FileName)"
                                   :alt="row.item.FeaturedImage == null ? '' : row.item.FeaturedImage.AltText" />
                        </div>
                    </template>
                    <template #cell(Title)="row">
                        <b v-if="row.item.PostStatus == 2">{{row.item.Title}}</b>
                        <b-link v-else
                                :to="{ name:'pages-article-edit', query: { edit : row.item.Id } }">
                            <b>{{row.item.Title}}</b>
                        </b-link>
                        <div class="row-actions">
                            <div v-if="isHovered(row.item) && isHiddenRowActions">
                                <b-link v-if="row.item.PostStatus != 0 && row.item.PostStatus != 2"
                                        :to="{ name:'pages-post-preview', query: { preview : row.item.Id } }"
                                        class="text-primary small">Önizle</b-link>
                                <b-link v-show="row.item.PostStatus == 0"
                                        :to="{ name:'pages-post-edit', query: { edit : row.item.Id } }"
                                        class="text-primary small">Görüntüle</b-link>
                                <small v-show="row.item.PostStatus != 2"
                                       class="text-muted"> | </small>
                                <b-link v-if="row.item.PostStatus != 2"
                                        :to="{ name:'pages-article-edit', query: { edit : row.item.Id } }"
                                        class="text-success small"
                                        variant="flat-danger">Düzenle</b-link>
                                <b-link v-else
                                        id="untrash"
                                        href="javascript:;"
                                        no-prefetch
                                        class="text-warning small"
                                        @click="postStatusChange($event, row.item.Id, row.item.Title)">Geri Al</b-link>
                                <small class="text-muted"> | </small>
                                <b-link v-if="row.item.PostStatus != 2"
                                        id="trash"
                                        href="javascript:;"
                                        no-prefetch
                                        class="text-danger small"
                                        @click="postStatusChange($event, row.item.Id, row.item.Title)">Çöp</b-link>
                                <b-link v-else
                                        href="javascript:;"
                                        no-prefetch
                                        class="text-danger small"
                                        @click="singleDeleteData(row.item.Id, row.item.Title)">Kalıcı Sil</b-link>
                            </div>
                        </div>
                    </template>
                    <template #cell(PostTerms.Category)="row">
                        <b-link v-for="postTerm in row.item.PostTerms"
                                v-if="postTerm.Term.TermType === 2"
                                :to="{ name:'pages-category-edit', query: { edit : postTerm.TermId } }">
                            <span class="small">{{ postTerm.Term.Name }}, </span>
                        </b-link>
                    </template>
                    <template #cell(PostTerms.Tag)="row">
                        <b-link v-for="postTerm in row.item.PostTerms"
                                v-if="postTerm.Term.TermType === 3"
                                :to="{ name:'pages-tag-edit', query: { edit : postTerm.TermId } }">
                            <span class="small">{{ postTerm.Term.Name }}, </span>
                        </b-link>
                    </template>
                    <template #cell(ModifiedDate)="row">
                        <b-badge v-if="row.item.PostStatus == 0"
                                 variant="success">
                            Yayında
                        </b-badge>
                        <b-badge v-if="row.item.PostStatus == 1"
                                 variant="warning">
                            Taslak
                        </b-badge>
                        <br />
                        <span class="small">{{ new Date(row.item.ModifiedDate).toLocaleString() }}</span>
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
    import moment from 'moment'
    import {
        BRow, BCol, BBreadcrumb, BBreadcrumbItem, BSpinner, BButton, BDropdown, BDropdownItem, BCard, BCardBody, BFormGroup, BInputGroup, BInputGroupPrepend, BInputGroupAppend, BFormInput, BFormCheckbox, VBTooltip, BTable, BImg, BBadge, BLink, BFormSelect, BPagination,
    } from 'bootstrap-vue'
    import axios from 'axios'
    import Ripple from 'vue-ripple-directive'

    export default {
        components: {
            moment,
            BRow,
            BCol,
            BBreadcrumb,
            BBreadcrumbItem,
            BSpinner,
            BButton,
            BDropdown,
            BDropdownItem,
            BCard,
            BCardBody,
            BFormGroup,
            BInputGroup,
            BInputGroupPrepend,
            BInputGroupAppend,
            BFormInput,
            BFormCheckbox,
            BTable,
            BImg,
            BBadge,
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
                        text: 'Makaleler',
                        active: true,
                    }
                ],
                noImage: require('@/assets/images/default/default-post-image.jpg'),
                isBusy: true,
                dataNullMessage: 'Hiçbir makale bulunamadı.',
                isShowSearchTextClearButton: false,
                filterText: '',
                posts: [],
                isHiddenStatusButton: false,
                isHiddenRowActions: false,
                pageColumnQuantity: '',
                fields: [
                    { key: 'Id', sortable: false, thStyle: { width: "20px" } },
                    { key: 'FeaturedImage', label: 'RESİM', sortable: false, thStyle: { width: "50px" } },
                    { key: 'Title', label: 'Başlık', sortable: true, thStyle: { width: "200px" } },
                    { key: 'PostTerms.Category', label: 'KATEGORİLER', sortable: true, thStyle: { width: "150px" } },
                    { key: 'PostTerms.Tag', label: 'ETİKETLER', sortable: true, thStyle: { width: "150px" } },
                    { key: 'ModifiedDate', label: 'TARİH', sortable: true, thStyle: { width: "150px" } },
                    { key: 'User.UserName', label: 'Yazar', sortable: true, thStyle: { width: "100px" } }
                ],
                selectAllCheck: false,
                checkedRows: Array,
                checkedRowsCount: '',
                publishPostsCount: '',
                draftPostsCount: '',
                trashPostsCount: '',
                hoveredRow: null,
                perPage: 10,
                pageOptions: [10, 20, 50, 100],
                totalRows: 1,
                currentPage: 1
            }
        },
        methods: {
            getAllData() {
                this.isBusy = true;
                axios.get('/admin/post-allposts', {
                    params: {
                        category: this.$route.query.category,
                        tag: this.$route.query.tag,
                        postType: 'article',
                        postStatus: this.$route.query.status
                    }
                })
                    .then((response) => {
                        if (response.data.PostListDto.ResultStatus === 0) {
                            this.posts = response.data.PostListDto.Data.Posts;
                            this.publishPostsCount = response.data.PublishPostsCount;
                            this.draftPostsCount = response.data.DraftPostsCount;
                            this.trashPostsCount = response.data.TrashPostsCount;
                            this.totalRows = response.data.PostListDto.Data.Posts.length;
                            this.pageColumnQuantity = this.perPage;
                        } else {
                            this.posts = [];
                            this.dataNullMessage = response.data.PostListDto.Message;
                        }
                        this.isBusy = false;
                        this.filterText = "";
                        this.allClear();
                    })
                    .catch((error) => {
                        console.log(error)
                        console.log(error.request)
                        this.$toast({
                            component: ToastificationContent,
                            props: {
                                variant: 'danger',
                                title: 'Hata Oluştu!',
                                icon: 'AlertOctagonIcon',
                                text: 'Makaleler listenirken hata oluştu. Lütfen tekrar deneyiniz.',
                            }
                        })
                    });
            },
            filterByName: function (data) {
                if (this.filterText.length === 0) {
                    return true;
                }
                return (data.Title.toLowerCase().indexOf(this.filterText.toLowerCase()) > -1);
            },
            allClear() {
                this.checkedRows = [];
                this.checkedRowsCount = '';
                this.isHiddenStatusButton = false;
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
                    this.isHiddenStatusButton = true;
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
                    this.isHiddenStatusButton = false;
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
                    this.isHiddenStatusButton = false;
                }
                this.checkChange();
            },
            changePage(e, pageNumber) {
                this.checkedRows = [];
                this.checkChange();
                this.pageColumnQuantity = this.perPage * pageNumber;
            },
            postStatusChange: function (event, id, name) {
                var postStatus = "";
                if (event.target.id == "trash") {
                    postStatus = "trash";
                } else {
                    postStatus = "draft";
                }

                axios.post('/admin/post-poststatuschange?postId=' + id + "&status=" + postStatus)
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
            },
            multiPostStatusChange: function (event) {
                var postStatus = "";
                if (event.target.id == "multi-publish") {
                    postStatus = "publish";
                } else if (event.target.id == "multi-draft") {
                    postStatus = "draft";
                } else if (event.target.id == "multi-untrash") {
                    postStatus = "draft";
                } else {
                    postStatus = "trash";
                }

                this.checkedRows.forEach((id, index) => {
                    axios.post('/admin/post-poststatuschange?postId=' + id + "&status=" + postStatus)
                        .then((response) => {
                            if (response.data.PostDto.ResultStatus === 0) {
                                this.getAllData();
                            }
                        });
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
                        axios.post('/admin/post-delete?postId=' + id)
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
                            axios.post('/admin/post-delete?postId=' + id)
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
        watch: {
            $route(to, from) {
                this.getAllData();
            }
        },
        computed: {
            filteredData: function () {
                return this.posts
                    .filter(this.filterByName);
            }
        },
        mounted() {
            this.getAllData();
        }
    }
</script>

<style>
    #article-list.card .card-header {
        padding: 8px 8px 8px 8px !important;
    }

    #article-table.table th, #article-table.table td {
        padding: 0.72rem !important;
    }

    #article-table > tbody > tr.b-table-bottom-row td {
        padding: 30px 0px 30px 0 !important;
    }

    .image-icon {
        width: 50px;
        height: 50px;
        position: relative;
    }

        .image-icon img {
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