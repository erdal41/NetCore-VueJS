<template>
    <b-row>
        <b-col md="12"
               lg="12">

            <b-card title="Tüm Makaleler"
                    header-tag="header"
                    no-body>
                <template #header>
                    <h3 class="modal-title">
                        Tüm Makaleler
                    </h3>
                    <div class="ml-auto">
                        <b-input-group size="sm">
                            <b-input-group-prepend is-text>
                                <feather-icon icon="SearchIcon" />
                            </b-input-group-prepend>
                            <b-form-input placeholder="Ara..."
                                          v-model="filterText" />
                        </b-input-group>
                    </div>
                    <div class="ml-auto">
                        <b-button v-show="publishPostsCount > 0"
                                  v-b-tooltip.hover
                                  title="Yayında olan makaleler"
                                  v-ripple.400="'rgba(113, 102, 240, 0.15)'"
                                  variant="fade-secondary"
                                  size="sm"
                                  @click="getAllPublishedData()">
                            <span class="text-primary">Yayında ( {{ publishPostsCount }} )</span>
                        </b-button>
                        <b-button v-show="draftPostsCount > -1"
                                  v-b-tooltip.hover
                                  title="Taslak olan makaleler"
                                  v-ripple.400="'rgba(113, 102, 240, 0.15)'"
                                  variant="fade-secondary"
                                  class="btn-icon"
                                  size="sm"
                                  @click="getAllDraftedData()">
                            <span class="text-warning">Taslak ( {{ draftPostsCount }} )</span>
                        </b-button>
                        <b-button v-show="trashPostsCount > -1"
                                  v-b-tooltip.hover
                                  title="Çöp kutusunda olan makaleler"
                                  v-ripple.400="'rgba(113, 102, 240, 0.15)'"
                                  variant="fade-secondary"
                                  class="btn-icon mr-1"
                                  size="sm"
                                  @click="getAllTrahedData()">
                            <span class="text-danger">Çöp ( {{ trashPostsCount }} )</span>
                        </b-button>
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
                <b-card-body>
                    <div class="d-flex justify-content-between flex-wrap">
                        <b-form-group v-if="isHiddenMultiDeleteButton === true"
                                      class="mb-0">
                            <b-button variant="danger"
                                      size="sm"
                                      @click="multiDeleteData">
                                <feather-icon icon="Trash2Icon"
                                              class="mr-50" />
                                <span class="align-middle">{{ checkedRowsCount }} Makaleyi Sil</span>
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
                <div v-if="isSpinnerShow == true"
                     class="text-center mt-2 mb-2">
                    <b-spinner variant="primary" />
                </div>
                <div v-else>
                    <b-table :items="filteredData"
                             :fields="fields"
                             :per-page="perPage"
                             :current-page="currentPage"
                             class="mb-0"
                             @row-hovered="rowHovered"
                             @row-unhovered="rowUnHovered">
                        <template #head(Id)="slot">
                            <b-form-checkbox @change="selectAllRows($event)"></b-form-checkbox>
                        </template>
                        <template #cell(Id)="row">
                            <b-form-checkbox :value="row.item.Id.toString()"
                                             :id="row.item.Id.toString()"
                                             v-model="checkedRows"
                                             @change="checkChange($event)"></b-form-checkbox>
                        </template>
                        <template #cell(FeaturedImage)="row">
                            <div class="image-thumb">
                                <b-img rounded
                                       v-bind:src="row.item.FeaturedImage.FileName == null ? noImage : require('@/assets/images/media/' + row.item.FeaturedImage.FileName)"
                                       :alt="row.item.FeaturedImage.AltText" />
                            </div>
                        </template>
                        <template #cell(Title)="row">
                            <b-link :to="{ name:'pages-term-edit', query: { edit : row.item.Id } }">
                                <b>{{row.item.Title}}</b>
                            </b-link>
                            <div class="row-actions">
                                <div v-if="isHovered(row.item) && isHiddenRowActions">
                                    <b-link :to="{ name:'pages-term-edit', query: { edit : row.item.Id } }"
                                            class="text-primary small">Görüntüle</b-link>
                                    <small class="text-muted"> | </small>
                                    <b-link :to="{ name:'pages-term-edit', query: { edit : row.item.Id } }"
                                            class="text-success small"
                                            variant="flat-danger">Düzenle</b-link>
                                    <small class="text-muted"> | </small>
                                    <b-link href="javascript:;"
                                            no-prefetch
                                            class="text-danger small"
                                            @click="singleDeleteData(row.item.Id, row.item.Title)">Sil</b-link>
                                </div>
                            </div>
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
                    </b-table>
                </div>
                <div v-show="posts.length <= 0"
                     class="text-center mt-1">{{ dataNullMessage  }}</div>
                <b-card-body>
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
                                      class="mb-0">
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
    import moment from 'moment'
    import { ValidationProvider, ValidationObserver, extend } from 'vee-validate'
    import { required, min, confirmed } from '@validations'
    import {
        BSpinner, BBadge, BTable, BFormCheckbox, BImg, BButton, BCard, BCardBody, BCardTitle, BRow, BCol, BForm, BFormSelect, BFormGroup, BFormTextarea, BPagination, BInputGroup, BFormInput, BInputGroupPrepend, VBTooltip, BLink
    } from 'bootstrap-vue'
    //import { codeRowDetailsSupport } from './code'
    import axios from 'axios'
    import ToastificationContent from '@core/components/toastification/ToastificationContent.vue'
    import vSelect from 'vue-select'
    import Ripple from 'vue-ripple-directive'

    extend('required', {
        ...required,
        message: 'Lütfen gerekli bilgileri yazınız.'
    });

    export default {
        components: {
            BSpinner,
            BBadge,
            BCardTitle,
            BForm,
            BFormSelect,
            BTable,
            BButton,
            BFormCheckbox,
            BImg,
            BFormTextarea,
            BCard,
            BRow,
            BCol,
            BCardBody,
            BFormGroup,
            BPagination,
            BInputGroup,
            BFormInput,
            BInputGroupPrepend,
            ToastificationContent,
            ValidationProvider,
            ValidationObserver,
            vSelect,
            BLink,
            moment
        },
        directives: {
            'b-tooltip': VBTooltip,
            Ripple,
        },
        data() {
            return {
                noImage: require('@/assets/images/default/default-post-image.jpg'),
                passValue: '',
                username: '',
                required,
                min,
                confirmed,
                isSpinnerShow: true,
                perPage: 10,
                pageOptions: [10, 20, 50, 100],
                totalRows: 1,
                currentPage: 1,
                filterText: '',
                posts: [],
                dataNullMessage: '',
                selected: '',
                selectedValue: null,
                isHiddenMultiDeleteButton: false,
                isHiddenRowActions: false,
                name: "",
                fields: [
                    { key: 'Id', sortable: false, thStyle: { width: "20px" } },
                    { key: 'FeaturedImage', label: 'Resim', sortable: false, thStyle: { width: "50px" } },
                    { key: 'Title', label: 'Başlık', sortable: true, thStyle: { width: "200px" } },
                    { key: 'ModifiedDate', label: 'Tarih', sortable: true, thStyle: { width: "150px" } },
                    { key: 'User.UserName', label: 'Yazar', sortable: true, thStyle: { width: "100px" } }],
                checkedRows: [],
                checkedRowsCount: 0,
                publishPostsCount:'',
                draftPostsCount:'',
                trashPostsCount:'',
                hoveredRow: null
            }
        },
        methods: {
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
            onChangeMethod(value) {
                this.termAddDto.ParentId = value;
            },
            checkChange() {
                if (this.checkedRows.length > 0) {
                    this.isHiddenMultiDeleteButton = true;
                    this.checkedRowsCount = this.checkedRows.length;
                }
                else {
                    this.isHiddenMultiDeleteButton = false;
                }
            },
            selectAllRows(value) {
                if (value === true) {
                    var idList = [];
                    this.posts.forEach(function (term) {
                        idList.push(term.Id);
                    });
                    this.checkedRows = idList;
                }
                else {
                    this.checkedRows = [];
                    this.isHidden = false;
                }
                this.checkChange();
            },
            singleDeleteData(id, name) {
                this.$swal({
                    title: 'Silmek istediğinize emin misiniz?',
                    text: name + " adlı terim kalıcı olarak silinecektir?",
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
                        axios.post('/admin/term/delete?term=' + id)
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
                    title: 'Silmek istediğinize emin misiniz?',
                    text: this.checkedRowsCount + " makale kalıcı olarak silinecektir?",
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
                        axios.post(`/admin/term/multidelete?posts=` + this.checkedRows)
                            .then((response) => {
                                console.log(response.data)
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
                                            text: response.data.TermDto.Message
                                        },
                                    })
                                }
                            })
                            .catch((error) => {
                                console.log(error)
                                console.log(error.response)
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
                    }
                })
                console.log(this.checkedRows);
            },
            getAllData() {
                this.isSpinnerShow = true;
                axios.get('/admin/post/allposts', {
                    params: {
                        post_type: 'article'
                    }
                })
                    .then((response) => {
                        console.log(response.data)
                        if (response.data.PostListDto.ResultStatus === 0) {
                            this.posts = response.data.PostListDto.Posts;
                            this.publishPostsCount = response.data.PublishPostsCount;
                            this.draftPostsCount = response.data.DraftPostsCount;
                            this.trashPostsCount = response.data.TrashPostsCount;
                            this.isSpinnerShow = false;
                        } else {
                            this.isSpinnerShow = false;
                            this.posts = [];
                            this.dataNullMessage = response.data.PostListDto.Message;
                        }
                    })
                    .catch((error) => {
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
            getAllPublishedData() {

            },
            filterByName: function (data) {
                // no search, don't filter :
                if (this.filterText.length === 0) {
                    return true;
                }

                return (data.Name.toLowerCase().indexOf(this.filterText.toLowerCase()) > -1);
            },
        },
        computed: {
            filteredData: function () {
                return this.posts
                    .filter(this.filterByName);
            }
        },
        mounted() {
            this.getAllData();
            this.totalRows = this.posts.length;
        }
    }
</script>

<style>
    [dir] .table th, [dir] .table td {
        padding: 0.72rem !important;
    }

    .image-thumb {
        width: 50px;
        height: 50px;
        position: relative;        
    }

        .image-thumb img {
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