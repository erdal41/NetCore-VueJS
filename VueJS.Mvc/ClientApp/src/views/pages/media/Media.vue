<template>
    <b-row>
        <b-col md="12"
               lg="4">
            <b-card title="Etiket Ekle">
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
                                                      placeholder="İsim" />
                                        <small class="text-danger">{{ errors[0] }}</small>
                                    </validation-provider>
                                </b-form-group>

                                <b-form-group label-for="slug"
                                              description="'slug' yazı isminin URL versiyonudur. Genellikle tümü küçük harflerden oluşur, sadece harf, rakam ve tire içerir.">
                                    <b-form-input id="slug"
                                                  v-model="termAddDto.Slug"
                                                  type="text"
                                                  placeholder="Kısa İsim" />
                                </b-form-group>

                                <b-form-textarea id="description"
                                                 v-model="termAddDto.Description"
                                                 placeholder="Açıklama"
                                                 rows="3" />

                                <!-- reset button -->
                                <b-button variant="primary"
                                          class="float-right mt-1"
                                          type="submit"
                                          @click.prevent="validationForm">
                                    Ekle
                                </b-button>
                            </b-col>
                        </b-row>
                    </b-form>
                </validation-observer>
            </b-card>
        </b-col>
        <b-col md="12"
               lg="8">

            <b-card title="Tüm Etiketler"
                    header-tag="header"
                    no-body>
                <template #header>
                    <h3 class="modal-title">
                        Tüm Etiketler
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
                                <span class="align-middle">{{ checkedRowsCount }} Etiketi Sil</span>
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
                    <b-table id="media-table"
                             :items="filteredData"
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

                        <template #cell(Name)="row">
                            <b-link :to="{ name:'pages-term-edit', query: { edit : row.item.Id } }">
                                <b>{{row.item.Name}}</b>
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
                                            @click="singleDeleteData(row.item.Id, row.item.Name)">Sil</b-link>
                                </div>
                            </div>
                        </template>
                    </b-table>
                </div>
                <div v-show="terms.length <= 0"
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
    import { ValidationProvider, ValidationObserver, extend } from 'vee-validate'
    import { required, min, confirmed } from '@validations'
    import {
        BSpinner, BTable, BFormCheckbox, BButton, BCard, BCardBody, BCardTitle, BRow, BCol, BForm, BFormSelect, BFormGroup, BFormTextarea, BPagination, BInputGroup, BFormInput, BInputGroupPrepend, VBTooltip, BLink
    } from 'bootstrap-vue'
    //import { codeRowDetailsSupport } from './code'
    import axios from 'axios'
    import ToastificationContent from '@core/components/toastification/ToastificationContent.vue'
    import vSelect from 'vue-select'
    import Ripple from 'vue-ripple-directive'
    import { integer } from '../../../../@core/utils/validations/validations'

    extend('required', {
        ...required,
        message: 'Lütfen gerekli bilgileri yazınız.'
    });

    export default {
        components: {
            BSpinner,
            BCardTitle,
            BForm,
            BFormSelect,
            BTable,
            BButton,
            BFormCheckbox,
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
            BLink
        },
        directives: {
            'b-tooltip': VBTooltip,
            Ripple,
        },
        data() {
            return {
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
                filterOnData: [],
                terms: [],
                dataNullMessage: '',
                selected: '',
                selectedValue: null,
                isHiddenMultiDeleteButton: false,
                isHiddenRowActions: false,
                name: "",
                fields: [
                    { key: 'Id', sortable: false, thStyle: { width: "20px" } },
                    { key: 'Name', label: 'İSİM', sortable: true, thStyle: { width: "200px" } },
                    { key: 'Description', label: 'Açıklama', sortable: true },
                    { key: 'Slug', label: 'KISA İSİM', sortable: true, thStyle: { width: "150px" } },
                    { key: 'Count', label: 'Toplam', sortable: true, thStyle: { width: "100px" } }],
                checkedRows: [],
                checkedRowsCount: 0,
                termAddDto: {
                    Name: "",
                    Slug: "",
                    ParentId: null,
                    Description: '',
                    TermType: 'tag'
                },
                seoObjectSettingAddDto: {
                    SeoTitle: this.Name
                },
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
                    this.terms.forEach(function (term) {
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
            validationForm() {
                this.$refs.simpleRules.validate().then(success => {
                    if (success) {
                        axios.post('/admin/term/new',
                            {
                                TermAddDto: this.termAddDto,
                                SeoObjectSettingAddDto: this.seoObjectSettingAddDto
                            })
                            .then((response) => {
                                console.log(response.data);
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
                            })
                            .catch((error) => {
                                console.log(error);
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
                    text: this.checkedRowsCount + " etiket kalıcı olarak silinecektir?",
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
                        axios.post(`/admin/term/multidelete?terms=` + this.checkedRows)
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
                axios.get('/admin/term/allterms', {
                    params: {
                        term_type: 'tag'
                    }
                })
                    .then((response) => {
                        console.log(response.data)
                        if (response.data.ResultStatus === 0) {
                            this.terms = response.data.Terms;
                            this.filterOnData = response.data.Terms;
                            this.isSpinnerShow = false;
                        } else {
                            this.isSpinnerShow = false;
                            this.terms = [];
                            this.dataNullMessage = response.data.Message;
                        }
                    })
                    .catch((error) => {
                        this.$toast({
                            component: ToastificationContent,
                            props: {
                                variant: 'danger',
                                title: 'Hata Oluştu!',
                                icon: 'AlertOctagonIcon',
                                text: this.title + ' listenirken hata oluştu. Lütfen tekrar deneyiniz.',
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
        },
        computed: {
            filteredData: function () {
                return this.terms
                    .filter(this.filterByName);
            }
        },
        mounted() {
            this.getAllData();
            this.totalRows = this.terms.length;
        }
    }
</script>

<style lang="scss">
    @import '@core/scss/vue/libs/vue-select.scss';

    [dir] .table th, [dir] .table td {
        padding: 0.72rem !important;
    }

    [dir] #media-table.table th:last-child, [dir] .table td:last-child {
        text-align: center;
    }
</style>