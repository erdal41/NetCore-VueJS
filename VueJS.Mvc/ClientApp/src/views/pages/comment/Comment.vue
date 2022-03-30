<template>
    <b-row>
        <modal-edit :comment-id="commentId"
                    @commentGetAllData="getAllData"></modal-edit>
        <modal-view :comment="viewComment"></modal-view>
        <b-col class="content-header-left mb-2"
               cols="12"
               md="6">
            <b-row class="breadcrumbs-top">
                <b-col cols="12">
                    <h2 class="content-header-title float-left pr-1 mb-0">
                        Yorumlar
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
        <b-col md="12"
               lg="12">
            <b-card id="comment-list"
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
                        <b-link v-b-tooltip.hover
                                title="Onaylanmış ve onay bekleyen yorumlar"
                                v-ripple.400="'rgba(113, 102, 240, 0.15)'"
                                variant="fade-secondary"
                                :to="{ name:'pages-comment-list'}"
                                class="text-primary small mr-1">Tümü ( {{ approvedCommentsCount + moderatedCommentsCount  }} )</b-link>
                        <b-link v-if="mineCommentsCount > 0"
                                v-b-tooltip.hover
                                title="Onaylanmış yorumlar"
                                v-ripple.400="'rgba(113, 102, 240, 0.15)'"
                                variant="fade-secondary"
                                :to="{ name:'pages-comment-list', query: { '' : 'mycomments' } , params: { userId : currentUser.Id } }"
                                class="text-primary small mr-1">Benim ( {{ mineCommentsCount }} )</b-link>
                        <b-link v-if="approvedCommentsCount > 0"
                                v-b-tooltip.hover
                                title="Onaylanmış yorumlar"
                                v-ripple.400="'rgba(113, 102, 240, 0.15)'"
                                variant="fade-secondary"
                                :to="{ name:'pages-comment-list', query: { status : 'approved' } }"
                                class="text-primary small mr-1">Onaylandı ( {{ approvedCommentsCount }} )</b-link>
                        <b-link v-if="moderatedCommentsCount > 0"
                                v-b-tooltip.hover
                                title="Onay bekleyen yorumlar"
                                v-ripple.400="'rgba(113, 102, 240, 0.15)'"
                                variant="fade-secondary"
                                :to="{ name:'pages-comment-list', query: { status : 'moderated' } }"
                                class="text-warning small mr-1">İnceleme Bekliyor ( {{ moderatedCommentsCount }} )</b-link>
                        <b-link v-if="trashCommentsCount > 0"
                                v-b-tooltip.hover
                                title="Çöp kutusunda olan yorumlar"
                                v-ripple.400="'rgba(113, 102, 240, 0.15)'"
                                variant="fade-secondary"
                                :to="{ name:'pages-comment-list', query: { status : 'trash' } }"
                                class="text-danger small mr-1">Çöp ( {{ trashCommentsCount }} )</b-link>
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
                <b-card-body v-if="isHiddenStatusButton === true && (($can('update', 'Comment') && $route.query.status !== 'trash') || ($can('delete', 'Comment') && $route.query.status === 'trash'))">
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
                                    <b-button v-if="$can('update', 'Comment') && $can('delete', 'Comment')"
                                              v-b-tooltip.hover
                                              title="Seçili yorumların durumlarını değiştirmenizi sağlar."
                                              v-ripple.400="'rgba(186, 191, 199, 0.15)'"
                                              variant="flat-secondary"
                                              size="sm"
                                              class="btn-icon rounded-circle ml-0">
                                        <feather-icon icon="InfoIcon" />
                                    </b-button>
                                </template>
                                <b-dropdown-item v-if="$can('update', 'Comment') && $route.query.status !== 'trash' && $route.query.status !== 'approved'"
                                                 href="javascript:;"
                                                 id="multi-approved"
                                                 variant="success"
                                                 @click="multiCommentStatusChange">
                                    Onayla
                                </b-dropdown-item>
                                <b-dropdown-item v-if="$can('update', 'Comment') && $route.query.status !== 'trash' && $route.query.status !== 'moderated'"
                                                 href="javascript:;"
                                                 id="multi-moderated"
                                                 variant="warning"
                                                 @click="multiCommentStatusChange">
                                    Beklemeye Al
                                </b-dropdown-item>

                                <b-dropdown-item v-if="$can('update', 'Comment') && $route.query.status !== 'trash'"
                                                 href="javascript:;"
                                                 id="multi-trash"
                                                 variant="danger"
                                                 @click="multiCommentStatusChange">
                                    Çöp Kutusuna Taşı
                                </b-dropdown-item>
                                <b-dropdown-item v-if="$can('update', 'Comment') && $route.query.status === 'trash'"
                                                 href="javascript:;"
                                                 id="multi-untrash"
                                                 variant="warning"
                                                 @click="multiCommentStatusChange">
                                    Geri Al
                                </b-dropdown-item>
                                <b-dropdown-item v-if="$can('delete', 'Comment') && $route.query.status === 'trash'"
                                                 href="javascript:;"
                                                 variant="danger"
                                                 @click="multiDeleteData">
                                    Kalıcı Olarak Sil
                                </b-dropdown-item>
                            </b-dropdown>
                        </b-form-group>
                    </div>
                </b-card-body>
                <b-table id="comment-table"
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
                        <b-form-checkbox :disabled="((!$can('update', 'Comment') && $route.query.status !== 'trash') || (!$can('delete', 'Comment') && $route.query.status === 'trash')) || filteredData.length <= 0"
                                         v-model="selectAllCheck"
                                         :value="true"
                                         @change="selectAllRows($event)"></b-form-checkbox>
                    </template>
                    <template #foot(Id)="slot">
                        <b-form-checkbox :disabled="((!$can('update', 'Comment') && $route.query.status !== 'trash') || (!$can('delete', 'Comment') && $route.query.status === 'trash')) || filteredData.length <= 0"
                                         v-model="selectAllCheck"
                                         :value="true"
                                         @change="selectAllRows($event)"></b-form-checkbox>
                    </template>
                    <template #cell(Id)="row">
                        <b-form-checkbox :disabled="((!$can('update', 'Comment') && $route.query.status !== 'trash') || (!$can('delete', 'Comment') && $route.query.status === 'trash'))"
                                         :value="row.item.Id.toString()"
                                         :id="row.item.Id.toString()"
                                         v-model="checkedRows"
                                         @change="checkChange($event)"></b-form-checkbox>
                    </template>
                    <template #cell(Name)="row">
                        <span class="font-weight-bolder"> {{ row.item.Name }}</span>
                        <br />
                        <b-link :href="'mailto:' + row.item.Email"><span> {{ row.item.Email }}</span></b-link>
                    </template>
                    <template #cell(Text)="row">
                        <small>{{row.item.Text}}</small>
                        <div class="row-actions">
                            <div v-if="isHovered(row.item) && isHiddenRowActions">
                                <b-link v-if="row.item.CommentStatus == 0 || row.item.CommentStatus == 1"
                                        v-b-modal.modal-view
                                        class="small"
                                        @click.prevent="viewClick(row.item.Name, row.item.Email, row.item.Text, row.item.CommentStatus)">Görüntüle</b-link>
                                <small v-if="row.item.CommentStatus !== 2 && $can('update', 'Comment')"
                                       class="text-muted"> | </small>
                                <b-link v-if="row.item.CommentStatus !== 2 && $can('update', 'Comment')"
                                        id="approved"
                                        class="text-success small"
                                        variant="flat-danger"
                                        @click.prevent="commentStatusChange($event, row.item.Id)">Onayla</b-link>
                                <small v-if="row.item.CommentStatus !== 2 && $can('update', 'Comment')"
                                       class="text-muted"> | </small>
                                <b-link v-if="row.item.CommentStatus !== 2 && $can('update', 'Comment')"
                                        v-b-modal.modal-edit
                                        class="text-secondary small"
                                        variant="flat-danger"
                                        @click.prevent=updateClick(row.item.Id)>Düzenle</b-link>
                                <small v-if="row.item.CommentStatus !== 2 && $can('update', 'Comment')"
                                       class="text-muted"> | </small>
                                <b-link v-if="row.item.CommentStatus !== 2 && $can('update', 'Comment')"
                                        id="trash"
                                        href="javascript:;"
                                        no-prefetch
                                        class="text-danger small"
                                        @click.prevent="commentStatusChange($event, row.item.Id)">Çöp</b-link>
                                <b-link v-if="row.item.CommentStatus === 2 && $can('update', 'Comment')"
                                        id="untrash"
                                        href="javascript:;"
                                        no-prefetch
                                        class="text-warning small"
                                        @click="commentStatusChange($event, row.item.Id)">Geri Al</b-link>
                                <small v-if="row.item.CommentStatus === 2 && $can('update', 'Comment') && $can('delete', 'Comment')"
                                       class="text-muted"> | </small>
                                <b-link v-if="row.item.CommentStatus === 2 && $can('delete', 'Comment')"
                                        href="javascript:;"
                                        no-prefetch
                                        class="text-danger small"
                                        @click="singleDeleteData(row.item.Id, row.item.Text)">Kalıcı Sil</b-link>
                            </div>
                        </div>
                    </template>
                    <template #cell(Post.Title)="row">
                        <b-link :to="{ name: 'pages-article-edit', query: { edit: row.item.Post.Id } }">
                            <span class="font-weight-bold">{{ row.item.Post.Title }}</span>
                        </b-link>
                        <br />
                        <b-link :to="{ name: 'pages-page-view', params: { postName : row.item.Post.PostName } }">
                            <small>Gönderiyi Görüntüle</small>
                        </b-link>
                    </template>
                    <template #cell(ModifiedDate)="row">
                        <b-badge v-if="row.item.CommentStatus == 1"
                                 variant="success">
                            Onaylandı
                        </b-badge>
                        <b-badge v-if="row.item.CommentStatus == 0"
                                 variant="warning">
                            İnceleme Bekliyor
                        </b-badge>
                        <br />
                        <span class="small">{{ new Date(row.item.ModifiedDate).toLocaleString() }}</span>
                    </template>
                    <template #cell(User.UserName)="row">
                        <b-link :to="{ name: 'pages-user-edit', query: { edit: row.item.User.Id } }">
                            <span class="font-weight-bold">{{ row.item.User.UserName }}</span>
                        </b-link>
                    </template>
                    <template v-if="filteredData.length <= 0"
                              slot="bottom-row">
                        <td colspan="6"
                            class="text-center">
                            {{ dataNullMessage  }}
                        </td>
                    </template>
                </b-table>
                <b-card-body v-if="comments.length > 0">
                    <div class="d-flex justify-content-between flex-wrap">
                        <!-- page length -->
                        <b-form-group label="Kayıt Sayısı: "
                                      label-cols="6"
                                      label-align="left"
                                      label-size="sm"
                                      label-for="perPageSelect"
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
    import ModalView from './ModalView.vue';
    import ModalEdit from './ModalEdit.vue';
    import moment from 'moment'
    import ToastificationContent from '@core/components/toastification/ToastificationContent.vue'
    import {
        BBreadcrumb, BBreadcrumbItem, BDropdown, BDropdownItem, BSpinner, BBadge, BTable, BFormCheckbox, BButton, BCard, BCardBody, BRow, BCol, BFormSelect, BFormGroup, BPagination, BInputGroup, BFormInput, BInputGroupPrepend, BInputGroupAppend, VBTooltip, BLink
    } from 'bootstrap-vue'
    //import { codeRowDetailsSupport } from './code'
    import axios from 'axios'

    import Ripple from 'vue-ripple-directive'

    export default {
        components: {
            ModalView,
            ModalEdit,
            moment,
            ToastificationContent,
            BBreadcrumb,
            BBreadcrumbItem,
            BDropdown,
            BDropdownItem,
            BSpinner,
            BBadge,
            BFormSelect,
            BTable,
            BButton,
            BFormCheckbox,
            BCard,
            BRow,
            BCol,
            BCardBody,
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
                        text: 'Yorumlar',
                        active: true,
                    }
                ],
                isBusy: true,
                dataNullMessage: 'Hiçbir yorum bulunamadı.',

                isShowSearchTextClearButton: false,
                filterText: '',
                comments: [],
                selected: '',
                selectedValue: null,
                isHiddenStatusButton: false,
                isHiddenRowActions: false,
                pageColumnQuantity: '',
                fields: [
                    { key: 'Id', sortable: false, thStyle: { width: "20px" } },
                    { key: 'Name', label: 'Yorum Yapan', sortable: true, thStyle: { width: "150px" } },
                    { key: 'Text', label: 'Yorum', sortable: true, thStyle: { width: "250px" } },
                    { key: 'Post.Title', label: 'Makale', sortable: true, thStyle: { width: "150px" } },
                    { key: 'ModifiedDate', label: 'Durum', sortable: true, thStyle: { width: "150px" } },
                    { key: 'User.UserName', label: 'Son Düzenleyen', sortable: true, thStyle: { width: "150px" } }
                ],
                selectAllCheck: false,
                checkedRows: [],
                checkedRowsCount: '',
                mineCommentsCount: '',
                approvedCommentsCount: '',
                moderatedCommentsCount: '',
                trashCommentsCount: '',
                hoveredRow: null,
                currentUser: '',
                perPage: 10,
                pageOptions: [10, 20, 50, 100],
                totalRows: 1,
                currentPage: 1,
                commentId: 0,
                viewComment: {
                    Name: '',
                    Email: '',
                    Text: '',
                    CommentStatus: ''
                },
            }
        },
        methods: {
            getAllData() {
                this.isBusy = true;
                axios.get('/admin/comment-allcomments', {
                    params: {
                        status: this.$route.query.status,
                        userId: this.$route.params.userId
                    }
                })
                    .then((response) => {
                        console.log(response.data)
                        if (response.data.CommentListDto.ResultStatus === 0) {
                            this.comments = response.data.CommentListDto.Data.Comments;
                            this.totalRows = response.data.CommentListDto.Data.Comments.length;
                            this.pageColumnQuantity = this.perPage;
                            this.currentUser = JSON.parse(localStorage.getItem('userData'));
                        } else {
                            this.comments = [];
                            this.dataNullMessage = response.data.CommentListDto.Message;
                        }
                        this.mineCommentsCount = response.data.MineCommentsCount;
                        this.approvedCommentsCount = response.data.ApprovedCommentsCount;
                        this.moderatedCommentsCount = response.data.ModeratedCommentsCount;
                        this.trashCommentsCount = response.data.TrashCommentsCount;

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
                                text: 'Yorumlar listenirken hata oluştu. Lütfen tekrar deneyiniz.',
                            }
                        })
                    });
            },
            filterByName: function (data) {
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
            changePage(e, pageNumber) {
                this.checkedRows = [];
                this.checkChange();
                this.pageColumnQuantity = this.perPage * pageNumber;
            },
            checkChange() {
                if (this.checkedRows.length > 0) {
                    this.isHiddenStatusButton = true;
                    this.checkedRowsCount = "( " + this.checkedRows.length + " )";

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
                    console.log(value)
                    console.log(this.pageColumnQuantity)
                    console.log(this.perPage)
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
            viewClick(name, mail, text, status) {
                this.viewComment.Name = name;
                this.viewComment.Email = mail;
                this.viewComment.Text = text;
                this.viewComment.CommentStatus = status;
            },
            updateClick(id) {
                this.commentId = id;
            },
            commentStatusChange: function (event, id) {
                var commentStatus = "";
                if (event.target.id == "trash") {
                    commentStatus = "trash";
                    console.log(commentStatus);
                } else if (event.target.id == "approved") {
                    commentStatus = "approved";
                    console.log(commentStatus);
                } else {
                    commentStatus = "moderated";
                    console.log(commentStatus);
                }

                axios.post('/admin/comment-commentstatuschange?commentId=' + id + "&commentStatus=" + commentStatus)
                    .then((response) => {
                        if (response.data.CommentDto.ResultStatus === 0) {
                            this.getAllData();
                            this.$toast({
                                component: ToastificationContent,
                                props: {
                                    variant: 'success',
                                    title: 'Başarılı İşlem!',
                                    icon: 'CheckIcon',
                                    text: response.data.CommentDto.Message
                                }
                            });
                        }
                        else {
                            this.$toast({
                                component: ToastificationContent,
                                props: {
                                    variant: 'danger',
                                    title: 'Başarısız İşlem!',
                                    icon: 'AlertOctagonIcon',
                                    text: response.data.CommentDto.Message
                                },
                            })
                        }
                    })
                    .catch((error) => {
                        console.log(error.request)
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
            multiCommentStatusChange: function (event) {
                var commentStatus = "";
                if (event.target.id == "multi-approved") {
                    commentStatus = "approved";
                } else if (event.target.id == "multi-moderated") {
                    commentStatus = "moderated";
                } else if (event.target.id == "multi-untrash") {
                    commentStatus = "moderated";
                } else {
                    commentStatus = "trash";
                }

                this.checkedRows.forEach((id, index) => {
                    axios.post('/admin/comment-commentstatuschange?commentId=' + id + "&commentStatus=" + commentStatus)
                        .then((response) => {
                            if (response.data.CommentDto.ResultStatus === 0) {
                                this.getAllData();
                            }
                        });
                })

            },
            singleDeleteData(id, name) {
                this.$swal({
                    title: 'Silmek istediğinize emin misiniz?',
                    text: name + " içerlikli yorum kalıcı olarak silinecektir?",
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
                        axios.post('/admin/comment-delete?commentId=' + id)
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
                    text: this.checkedRowsCount + " adet yorum kalıcı olarak silinecektir?",
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
                            axios.post('/admin/comment-delete?commentId=' + id)
                                .then((response) => {
                                    if (response.data.ResultStatus === 0) {
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
                return this.comments
                    .filter(this.filterByName);
            }
        },
        mounted() {
            this.getAllData();
        }
    }
</script>

<style>
    #comment-list.card .card-header {
        padding: 8px 8px 8px 8px;
    }

    #comment-table.table th, #comment-table.table td {
        padding: 0.72rem !important;
    }

    #comment-table > tbody > tr.b-table-bottom-row td {
        padding: 30px 0px 30px 0 !important;
    }
</style>