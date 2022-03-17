<template>
    <div>
        <b-row>
            <b-col class="content-header-left mb-2"
                   cols="6"
                   md="6">
                <b-row class="breadcrumbs-top">
                    <b-col cols="12">
                        <h2 class="content-header-title float-left pr-1 mb-0">
                            Menüler
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
                <b-button v-if="$can('create', 'Otherpage') && menuId != ''"
                          variant="outline-danger"
                          class="mr-1"
                          @click.prevent="deleteMenu">
                    Sil
                </b-button>
                <b-button v-if="$can('create', 'Otherpage')"
                          variant="primary"
                          @click.prevent="addMenu">
                    Kaydet
                </b-button>
            </b-col>
        </b-row>
        <b-row class="menu-banner">
            <b-col cols="3">
                <label label="Düzenlemek için bir menü seçin"
                       label-for="tag"
                       class="col-form-label">
                    Düzenlemek için bir menü seçin
                </label>
            </b-col>
            <b-col cols="3"
                   class="menu-select">
                <v-select v-model="menuId"
                          :options="menus"
                          label="Name"
                          :reduce="(option) => option.Id"
                          :clearable="false"
                          placeholder="— Seçim Yapın —"
                          :disabled="isSelectLoading"
                          :loading="isSelectLoading"
                          @input="getAllMenuDetails">
                    <template #spinner="{ loading }">
                        <div v-if="isSelectLoading"
                             style="border-left-color: rgba(88, 151, 251, 0.71)"
                             class="vs__spinner">
                        </div>
                    </template>
                    <template #no-options="{ search, searching, loading }">
                        {{ nullMenuMessage }}
                    </template>
                </v-select>
            </b-col>
            <b-col cols="6">
                <b-form-group label="veya yeni bir menü ekleyin"
                              label-for="tag"
                              label-cols="5"
                              class="m-0">
                    <validation-observer v-if="menuId == ''"
                                         ref="addMenuValidation">
                        <validation-provider #default="{ errors }"
                                             name="newMenuName"
                                             vid="newMenuName"
                                             rules="required">
                            <b-form-input id="newMenuName"
                                          v-model="newMenuName"
                                          :state="errors.length > 0 ? false:null"
                                          type="text"
                                          placeholder="Menü ismi giriniz." />
                            <small class="text-danger">{{ errors[0] }}</small>
                        </validation-provider>
                    </validation-observer>
                    <b-button v-if="$can('create','Tag') && menuId != ''"
                              variant="outline-primary"
                              @click.prevent="menuId = ''; newMenuName = ''; updateMenuName = ''; menuDetails = []">Yeni Menü Oluştur</b-button>
                </b-form-group>
            </b-col>
        </b-row>
        <b-row class="mt-3">
            <b-col cols="3">
                <h4>Menü Öğeleri Ekle</h4>
                <app-collapse id="menu-items"
                              type="margin"
                              accordion>
                    <app-collapse-item title="Sayfalar"
                                       visible>
                        <div class="menu-item-search">
                            <b-form-input v-model="pageFilterText"
                                          size="sm"
                                          placeholder="Ara.."></b-form-input>
                        </div>
                        <div class="menu-item-list">
                            <span v-if="filteredPage.length < 1">Sayfa bulunamadı.</span>
                            <b-form-checkbox v-else
                                             v-for="page in filteredPage"
                                             :key="page.Id"
                                             v-model="selectedPageItem"
                                             :value="page"
                                             name="page.Title"
                                             class="check-page"
                                             @change="pageCheckChange">
                                {{ page.Title }}
                            </b-form-checkbox>
                        </div>
                        <div class="menu-items-action">
                            <b-form-checkbox v-model="allPagesCheck"
                                             :value="true"
                                             @change="allPageCheckChange">
                                Tümünü Seç
                            </b-form-checkbox>
                            <b-button variant="outline-primary"
                                      size="sm"
                                      class="float-right"
                                      @click.prevent="menuAddPage">
                                Menüye Ekle
                            </b-button>
                        </div>
                    </app-collapse-item>
                    <app-collapse-item title="Makaleler">
                        <div class="menu-item-search">
                            <b-form-input v-model="articleFilterText"
                                          size="sm"
                                          placeholder="Ara.."></b-form-input>
                        </div>
                        <div class="menu-item-list">
                            <span v-if="filteredArticle.length < 1">Makale bulunamadı.</span>
                            <b-form-checkbox v-else
                                             v-for="article in filteredArticle"
                                             :key="article.Id"
                                             v-model="selectedArticleItem"
                                             :value="article"
                                             name="article.Title"
                                             class="check-article"
                                             @change="articleCheckChange">
                                {{ article.Title }}
                            </b-form-checkbox>
                        </div>
                        <div class="menu-items-action">
                            <b-form-checkbox v-model="allArticlesCheck"
                                             :value="true"
                                             @change="allArticleCheckChange">
                                Tümünü Seç
                            </b-form-checkbox>
                            <b-button variant="outline-primary"
                                      size="sm"
                                      class="float-right"
                                      @click.prevent="menuAddArticle">
                                Menüye Ekle
                            </b-button>
                        </div>
                    </app-collapse-item>
                    <app-collapse-item title="Kategoriler">
                        <div class="menu-item-search">
                            <b-form-input v-model="categoryFilterText"
                                          size="sm"
                                          placeholder="Ara.."></b-form-input>
                        </div>
                        <div class="menu-item-list">
                            <span v-if="filteredCategory.length < 1">Kategori bulunamadı.</span>
                            <b-form-checkbox v-else
                                             v-for="category in filteredCategory"
                                             :key="category.Id"
                                             v-model="selectedCategoryItem"
                                             :value="category"
                                             name="category.Name"
                                             class="check-category"
                                             @change="categoryCheckChange">
                                {{ category.Name }}
                            </b-form-checkbox>
                        </div>
                        <div class="menu-items-action">
                            <b-form-checkbox v-model="allCategoriesCheck"  
                                             :value="true"
                                             @change="allCategoryCheckChange">
                                Tümünü Seç
                            </b-form-checkbox>
                            <b-button variant="outline-primary"
                                      size="sm"
                                      class="float-right"
                                      @click.prevent="menuAddCategory">
                                Menüye Ekle
                            </b-button>
                        </div>
                    </app-collapse-item>
                    <app-collapse-item title="Özel Bağlantılar">
                        <div class="custom-menu-item">
                            <b-form-group label="İsim"
                                          label-for="custom-name">
                                <b-form-input v-model="customName"
                                              id="custom-name"
                                              size="sm"
                                              placeholder="Ara.."></b-form-input>
                            </b-form-group>
                            <b-form-group label="URL"
                                          label-for="custom-url">
                                <b-form-input v-model="customURL"
                                              id="custom-url"
                                              size="sm"
                                              placeholder="Ara.."></b-form-input>
                            </b-form-group>
                            <b-button variant="outline-primary"
                                      size="sm"
                                      class="float-right mb-1"
                                      @click.prevent="menuAddCustom">
                                Menüye Ekle
                            </b-button>
                        </div>
                    </app-collapse-item>
                </app-collapse>
            </b-col>
            <b-col cols="9">
                <b-card class="min-vh-100">
                    <b-form-group v-if="menuId != ''"
                                  label="Menü Adı"
                                  label-for="updateMenuName">
                        <b-form-input id="updateMenuName"
                                      v-model="updateMenuName"></b-form-input>
                        <hr />
                    </b-form-group>
                    <b-form-group v-if="menuDetails.length > 0">
                        <b-form-checkbox v-model="isAllCheckMenuItem"
                                         :value="true"
                                         @change="deleteMenuItemList = []; deleteMenuItemDisplay = []">
                            Toplu Seçim
                        </b-form-checkbox>
                    </b-form-group>

                    <nested-draggable v-if="menuDetails.length > 0"
                                      class="col-8" v-model="menuDetails"
                                      :list="menuDetails"
                                      :isMenuItemCheck="isAllCheckMenuItem"
                                      @menuItemDeleted="deleteMenuItem"
                                      @deleteMenuItem="deleteMenuItems" />

                    <b-form-group v-if="menuDetails.length > 0"
                                  class="mt-1">
                        <b-button variant="flat-danger"
                                  size="sm"
                                  class="ml-1"
                                  :disabled="deleteMenuItemList.length < 1"
                                  @click.prevent="multiMenuItemDelete">
                            Seçili Öğeleri Kaldır
                        </b-button>
                    </b-form-group>
                    <span v-if="deleteMenuItemList.length > 0">
                        Silme için seçilen menü öğeleri listesi:
                    </span>
                    <p v-if="deleteMenuItemList.length > 0">
                        {{ deleteMenuItemDisplay.toString() }}
                    </p>
                    <pre>
                        {{ menuDetails }}
                    </pre>
                </b-card>
            </b-col>
        </b-row>
    </div>
</template>

<script>
    import NestedDraggable from "./components/NestedDraggable";
    import draggable from "vuedraggable";
    import AppCollapse from '@core/components/app-collapse/AppCollapse.vue'
    import AppCollapseItem from '@core/components/app-collapse/AppCollapseItem.vue'
    import { ValidationProvider, ValidationObserver, extend } from 'vee-validate'
    import { required } from '@validations'
    import { BRow, BCol, BBreadcrumb, BBreadcrumbItem, BButton, BSpinner, BFormCheckbox, BCard, BFormGroup, BFormInput, BListGroupItem } from 'bootstrap-vue'

    import axios from 'axios'
    import ToastificationContent from '@core/components/toastification/ToastificationContent.vue'
    import vSelect from 'vue-select'

    extend('required', {
        ...required,
        message: 'Lütfen menü adını giriniz.'
    });

    export default {
        components: {
            NestedDraggable,
            draggable,
            AppCollapse,
            AppCollapseItem,
            ToastificationContent,
            ValidationProvider,
            ValidationObserver,
            vSelect,
            BRow,
            BCol,
            BBreadcrumb,
            BBreadcrumbItem,
            BSpinner,
            BButton,
            BFormCheckbox,
            BCard,
            BFormGroup,
            BFormInput,
            BListGroupItem,
        },
        data() {
            return {
                breadcrumbs: [
                    {
                        text: 'Menüler',
                        active: true,
                    }
                ],
                required,
                isSelectLoading: false,
                isSpinnerShow: true,
                isShowSearchTextClearButton: false,
                nullMenuMessage: '',
                menuId: '',
                newMenuName: '',
                updateMenuName: '',
                menus: [],
                menuDetails: [],
                pages: [],
                articles: [],
                categories: [],
                pageFilterText: '',
                articleFilterText: '',
                categoryFilterText: '',
                selectedPageItem: [],
                selectedArticleItem: [],
                selectedCategoryItem: [],
                allPagesCheck: false,
                allArticlesCheck: false,
                allCategoriesCheck: false,
                dataNullMessage: '',
                selectAllCheck: false,
                isAllCheckMenuItem: false,
                checkedRows: [],
                checkedRowsCount: '',
                menuDetailUpdateDto: {
                    Id: '',
                    Name: "",
                },
                customName: '',
                customURL: '',
                deleteMenuItemList: [],
                deleteMenuItemDisplay: []
            }
        },
        methods: {
            deleteMenuItems(checkMenuItems) {
                this.deleteMenuItemList = checkMenuItems;
                this.deleteMenuItemDisplay = [];
                if (checkMenuItems.length > 0) {
                    checkMenuItems.forEach(item => {
                        this.deleteMenuItemDisplay.push(item.Name)
                    });
                }
                else {
                    this.deleteMenuItemList = [];
                    this.deleteMenuItemDisplay = [];
                }
            },
            pageCheckChange() {
                if (this.pages.length == this.selectedPageItem.length) {
                    this.allPagesCheck = true;
                }
                else {
                    this.allPagesCheck = false;
                }
            },
            articleCheckChange() {
                if (this.articles.length == this.selectedArticleItem.length) {
                    this.allArticlesCheck = 'true';
                }
                else {
                    this.allArticlesCheck = 'false';
                }
            },
            categoryCheckChange() {
                if (this.categories.length == this.selectedCategoryItem.length) {
                    this.allCategoriesCheck = 'true';
                }
                else {
                    this.allCategoriesCheck = 'false';
                }
            },
            allPageCheckChange(value) {
                if (value === true) {
                    this.pages.forEach(page => {
                        this.selectedPageItem.push(page);
                    });
                } else {
                    this.selectedPageItem = [];
                }
            },
            allArticleCheckChange(value) {
                if (value === true) {
                    this.articles.forEach(article => {
                        this.selectedArticleItem.push(article);
                    });
                } else {
                    this.selectedArticleItem = [];
                }
            },
            allCategoryCheckChange(value) {
                if (value === true) {
                    this.categories.forEach(category => {
                        this.selectedCategoryItem.push(category);
                    });
                } else {
                    this.selectedCategoryItem = [];
                }
            },
            addMenu() {
                if (this.menuId === '') {
                    this.$refs.addMenuValidation.validate().then(success => {
                        if (success) {
                            var menuAddDto = {
                                Name: this.newMenuName
                            }
                            axios.post('/admin/menu-new', {
                                MenuAddDto: menuAddDto
                            }).then((response) => {
                                if (response.data.MenuDto.ResultStatus === 0) {
                                    this.getAllMenus();
                                    this.menuId = response.data.MenuDto.Data.Menu.Id
                                    this.updateMenuName = response.data.MenuDto.Data.Menu.Name

                                    this.$toast({
                                        component: ToastificationContent,
                                        props: {
                                            variant: 'success',
                                            title: 'Başarılı İşlem!',
                                            icon: 'CheckIcon',
                                            text: response.data.MenuDto.Message
                                        }
                                    });
                                }
                            });
                        }
                    });
                }
                else {
                    var menuUpdateDto = {
                        Id: this.menuId,
                        Name: this.updateMenuName
                    }
                    axios.post('/admin/menu-edit', {
                        MenuUpdateDto: menuUpdateDto
                    }).then((response) => {
                        if (response.data.MenuDto.ResultStatus === 0) {

                            for (var i = 0; i < this.menuDetails.length; i++) {

                                var menuDetailUpdateDto = {
                                    Id: this.menuDetails[i].Id,
                                    CustomName: this.menuDetails[i].CustomName,
                                    CustomURL: this.menuDetails[i].CustomURL,
                                    ParentId: this.menuDetails[i].ParentId,
                                    MenuId: this.menuId,
                                    ObjectId: this.menuDetails[i].ObjectId,
                                    SubObjectType: this.menuDetails[i].SubObjectType,
                                    MenuOrder: i + 1,
                                    Children: this.menuDetails[i].Children,
                                    Parent: this.menuDetails[i].Parent,
                                    Parents: this.menuDetails[i].Parents,
                                }

                                axios.post('/admin/menu-editmenudetail', {
                                    MenuDetailUpdateDto: menuDetailUpdateDto
                                }).then((res) => {
                                    if (res.data.MenuDetailDto.ResultStatus === 0) {
                                        console.log('OLEYYYYYYYYYYYYYY')
                                    }
                                });

                                if (this.menuDetails[i].Children.length > 0) {
                                    for (var j = 0; j < this.menuDetails[i].Children.length; j++) {
                                        var menuDetailChildrenUpdateDto = {
                                            Id: this.menuDetails[i].Children[j].Id,
                                            CustomName: this.menuDetails[i].Children[j].CustomName,
                                            CustomURL: this.menuDetails[i].Children[j].CustomURL,
                                            ParentId: this.menuDetails[i].Id,
                                            MenuId: this.menuId,
                                            ObjectId: this.menuDetails[i].Children[j].ObjectId,
                                            SubObjectType: this.menuDetails[i].Children[j].SubObjectType,
                                            MenuOrder: j + 1,
                                            Children: this.menuDetails[i].Children[j].Children,
                                            Parent: this.menuDetails[i].Children[j].Parent,
                                            Parents: this.menuDetails[i].Children[j].Parents,
                                        }

                                        axios.post('/admin/menu-editmenudetail', {
                                            MenuDetailUpdateDto: menuDetailChildrenUpdateDto
                                        }).then((res) => {
                                            if (res.data.MenuDetailDto.ResultStatus === 0) {
                                                console.log('OLEYYYYYYYYYYYYYY 2 ')
                                            }
                                        });
                                    }

                                }

                            }                         

                            this.getAllMenus();
                            this.$toast({
                                component: ToastificationContent,
                                props: {
                                    variant: 'success',
                                    title: 'Başarılı İşlem!',
                                    icon: 'CheckIcon',
                                    text: response.data.MenuDto.Message
                                }
                            });
                        }
                    });
                }
            },
            menuAddPage() {
                if (this.menuId == '') {
                    this.$toast({
                        component: ToastificationContent,
                        props: {
                            variant: 'warning',
                            title: 'Uyarı!',
                            icon: 'AlertTriangleIcon',
                            text: 'Lütfen bir menü seçiniz.'
                        }
                    });
                }
                else {
                    this.selectedPageItem.forEach(item => {
                        var menuDetailAddDto = {
                            CustomName: item.Title,
                            CustomURL: item.PostName,
                            ParentId: null,
                            MenuId: this.menuId,
                            ObjectId: item.Id,
                            SubObjectType: 0,
                            MenuOrder: this.menuDetails.length + 1
                        };

                        axios.post('/admin/menu-newmenudetail', {
                            MenuDetailAddDto: menuDetailAddDto
                        }).then((response) => {
                            if (response.data.MenuDetailDto.ResultStatus === 0) {
                                this.selectedPageItem = [];
                                this.allPagesCheck =false;

                                this.menuDetails.push({
                                    Id: response.data.MenuDetailDto.Data.MenuDetail.Id,
                                    CustomName: response.data.MenuDetailDto.Data.MenuDetail.CustomName,
                                    CustomURL: response.data.MenuDetailDto.Data.MenuDetail.CustomURL,
                                    MenuId: this.menuId,
                                    ObjectId: response.data.MenuDetailDto.Data.MenuDetail.ObjectId,
                                    MenuOrder: response.data.MenuDetailDto.Data.MenuDetail.MenuOrder,
                                    SubObjectType: 0,
                                    Children: response.data.MenuDetailDto.Data.MenuDetail.Children,
                                    Parent: response.data.MenuDetailDto.Data.MenuDetail.Parent,
                                    ParentId: response.data.MenuDetailDto.Data.MenuDetail.ParentId,
                                    Parents: response.data.MenuDetailDto.Data.MenuDetail.Parents,
                                });
                            }
                        });
                    });
                }
            },
            menuAddArticle() {
                if (this.menuId == '') {
                    this.$toast({
                        component: ToastificationContent,
                        props: {
                            variant: 'warning',
                            title: 'Uyarı!',
                            icon: 'AlertTriangleIcon',
                            text: 'Lütfen bir menü seçiniz.'
                        }
                    });
                }
                else {
                    this.selectedArticleItem.forEach(item => {
                        var menuDetailAddDto = {
                            CustomName: item.Title,
                            CustomURL: item.PostName,
                            ParentId: null,
                            MenuId: this.menuId,
                            ObjectId: item.Id,
                            SubObjectType: 1,
                            MenuOrder: this.menuDetails.length + 1
                        };

                        axios.post('/admin/menu-newmenudetail', {
                            MenuDetailAddDto: menuDetailAddDto
                        }).then((response) => {
                            if (response.data.MenuDetailDto.ResultStatus === 0) {
                                this.selectedArticleItem = [];
                                this.allArticlesCheck = false;

                                this.menuDetails.push({
                                    Id: response.data.MenuDetailDto.Data.MenuDetail.Id,
                                    CustomName: response.data.MenuDetailDto.Data.MenuDetail.CustomName,
                                    CustomURL: response.data.MenuDetailDto.Data.MenuDetail.CustomURL,
                                    MenuId: this.menuId,
                                    ObjectId: response.data.MenuDetailDto.Data.MenuDetail.ObjectId,
                                    MenuOrder: response.data.MenuDetailDto.Data.MenuDetail.MenuOrder,
                                    SubObjectType: 1,
                                    Children: response.data.MenuDetailDto.Data.MenuDetail.Children,
                                    Parent: response.data.MenuDetailDto.Data.MenuDetail.Parent,
                                    ParentId: response.data.MenuDetailDto.Data.MenuDetail.ParentId,
                                    Parents: response.data.MenuDetailDto.Data.MenuDetail.Parents,
                                });
                            }
                        });
                    });
                }
            },
            menuAddCategory() {
                if (this.menuId == '') {
                    this.$toast({
                        component: ToastificationContent,
                        props: {
                            variant: 'warning',
                            title: 'Uyarı!',
                            icon: 'AlertTriangleIcon',
                            text: 'Lütfen bir menü seçiniz.'
                        }
                    });
                }
                else {
                    this.selectedCategoryItem.forEach(item => {
                        var menuDetailAddDto = {
                            CustomName: item.Name,
                            CustomURL: item.Slug,
                            ParentId: null,
                            MenuId: this.menuId,
                            ObjectId: item.Id,
                            SubObjectType: 2,
                            MenuOrder: this.menuDetails.length + 1
                        };

                        axios.post('/admin/menu-newmenudetail', {
                            MenuDetailAddDto: menuDetailAddDto
                        }).then((response) => {
                            if (response.data.MenuDetailDto.ResultStatus === 0) {
                                this.selectedCategoryItem = [];
                                this.allCategoriesCheck = false;

                                this.menuDetails.push({
                                    Id: response.data.MenuDetailDto.Data.MenuDetail.Id,
                                    CustomName: response.data.MenuDetailDto.Data.MenuDetail.CustomName,
                                    CustomURL: response.data.MenuDetailDto.Data.MenuDetail.CustomURL,
                                    MenuId: this.menuId,
                                    ObjectId: response.data.MenuDetailDto.Data.MenuDetail.ObjectId,
                                    MenuOrder: response.data.MenuDetailDto.Data.MenuDetail.MenuOrder,
                                    SubObjectType: 2,
                                    Children: response.data.MenuDetailDto.Data.MenuDetail.Children,
                                    Parent: response.data.MenuDetailDto.Data.MenuDetail.Parent,
                                    ParentId: response.data.MenuDetailDto.Data.MenuDetail.ParentId,
                                    Parents: response.data.MenuDetailDto.Data.MenuDetail.Parents,
                                });                                                               
                            }
                        });
                    });
                }
            },
            menuAddCustom() {
                if (this.menuId == '') {
                    this.$toast({
                        component: ToastificationContent,
                        props: {
                            variant: 'warning',
                            title: 'Uyarı!',
                            icon: 'AlertTriangleIcon',
                            text: 'Lütfen bir menü seçiniz.'
                        }
                    });
                }
                else {
                    var menuDetailAddDto = {
                        CustomName: this.customName,
                        CustomURL: this.customURL,
                        ParentId: null,
                        MenuId: this.menuId,
                        ObjectId: null,
                        SubObjectType: null,
                        MenuOrder: this.menuDetails.length + 1
                    };
                    axios.post('/admin/menu-newmenudetail', {
                        MenuDetailAddDto: menuDetailAddDto
                    }).then((response) => {
                        if (response.data.MenuDetailDto.ResultStatus === 0) {
                            this.menuDetails.push({
                                Id: response.data.MenuDetailDto.Data.MenuDetail.Id,
                                CustomName: response.data.MenuDetailDto.Data.MenuDetail.CustomName,
                                CustomURL: response.data.MenuDetailDto.Data.MenuDetail.CustomURL,
                                MenuId: this.menuId,
                                ObjectId: '',
                                MenuOrder: response.data.MenuDetailDto.Data.MenuDetail.MenuOrder,
                                SubObjectType: null,
                                Children: response.data.MenuDetailDto.Data.MenuDetail.Children,
                                Parent: response.data.MenuDetailDto.Data.MenuDetail.Parent,
                                ParentId: response.data.MenuDetailDto.Data.MenuDetail.ParentId,
                                Parents: response.data.MenuDetailDto.Data.MenuDetail.Parents,
                            });
                            this.customURL = '';
                            this.customName = '';                            
                        }
                    }).catch((error) => {
                        console.log(error)
                        console.log(error.request)
                    });
                }
            },
            deleteMenu() {
                this.$swal({
                    title: 'Silmek istediğinize emin misiniz?',
                    text: this.updateMenuName + " adlı menü kalıcı olarak silinecektir?",
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
                        axios.post('/admin/menu-delete?menuId=' + this.menuId)
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
                                    this.getAllMenus();
                                    this.menuDetails = [];
                                    this.selectedPageItem = [];
                                    this.selectedArticleItem = [];
                                    this.selectedCategoryItem = [];
                                    this.CustomURL = '';
                                    this.customName = '';
                                    this.menuId = '';
                                    this.newMenuName = '';
                                    this.updateMenuName = '';
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
            multiMenuItemDelete() {
                if (this.deleteMenuItemList.length > 0) {
                    this.deleteMenuItemList.forEach(item => {
                        axios.post('/admin/menu-deletemenudetail?menuId=' + item.Id).then((response) => {
                            if (response.data.ResultStatus === 0) {
                                this.deleteMenuItem(item.Id);
                                this.deleteMenuItemList = [];
                                this.deleteMenuItemDisplay = [];
                                this.isAllCheckMenuItem = false;
                            }
                        });
                    })
                }                
            },
            deleteMenuItem(id) {
                this.menuDetails = this.menuDetails.filter(item => item.Id != id)
            },
            getAllMenus() {
                this.menus = [];
                this.isSelectLoading = true;
                axios.get('/admin/menu-allmenus')
                    .then((response) => {
                        console.log(response.data)
                        if (response.data.MenuListDto.ResultStatus === 0) {
                            this.menus = response.data.MenuListDto.Data.Menus;
                            this.isSelectLoading = false;
                        }
                        else {
                            this.menus = [];
                            this.nullMenuMessage = response.data.MenuListDto.Message;
                            this.isSelectLoading = false;
                        }
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
                                text: 'Menüler listenirken hata oluştu. Lütfen tekrar deneyiniz.',
                            }
                        })
                    });
            },
            getAllMenuDetails(id) {
                this.updateMenuName = this.menus.filter(menu => menu.Id == id)[0].Name;
                axios.get('/admin/menu-allmenudetails', {
                    params: {
                        menuId: this.menuId
                    }
                })
                    .then((response) => {
                        console.log(response.data)
                        if (response.data.MenuDetailListDto.ResultStatus === 0) {
                            this.menuDetails = response.data.MenuDetailListDto.Data.MenuDetails;
                            this.deleteMenuItemList = [];
                            this.isAllCheckMenuItem = false;
                        }
                        else {
                            this.menuDetails = [];
                        }
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
                                text: 'Menü içeriği listenirken hata oluştu. Lütfen tekrar deneyiniz.',
                            }
                        })
                    });
            },
            getAllPages() {
                axios.get('/admin/post-allposts', {
                    params: {
                        postType: 'page',
                        postStatus: 'publish'
                    }
                })
                    .then((response) => {
                        if (response.data.PostListDto.ResultStatus === 0) {
                            this.pages = response.data.PostListDto.Data.Posts;
                        } else {
                            this.pages = [];
                        }
                    })
                    .catch((error) => {
                        this.$toast({
                            component: ToastificationContent,
                            props: {
                                variant: 'danger',
                                title: 'Hata Oluştu!',
                                icon: 'AlertOctagonIcon',
                                text: 'Sayfalar listenirken hata oluştu. Lütfen tekrar deneyiniz.',
                            }
                        })
                    });
            },
            getAllArticles() {
                axios.get('/admin/post-allposts', {
                    params: {
                        postType: 'article',
                        postStatus: 'publish'
                    }
                })
                    .then((response) => {
                        if (response.data.PostListDto.ResultStatus === 0) {
                            this.articles = response.data.PostListDto.Data.Posts;
                        } else {
                            this.articles = [];
                        }
                    })
                    .catch((error) => {
                        this.$toast({
                            component: ToastificationContent,
                            props: {
                                variant: 'danger',
                                title: 'Hata Oluştu!',
                                icon: 'AlertOctagonIcon',
                                text: 'Sayfalar listenirken hata oluştu. Lütfen tekrar deneyiniz.',
                            }
                        })
                    });
            },
            getAllCategories() {
                axios.get('/admin/term-allterms', {
                    params: {
                        termType: "category"
                    }
                })
                    .then((response) => {
                        if (response.data.TermListDto.ResultStatus === 0) {
                            this.categories = response.data.TermListDto.Data.Terms;
                        }
                        else {
                            this.categories = [];
                        }
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
            pagesFilterByTitle: function (data) {
                // no search, don't filter :
                if (this.pageFilterText.length === 0) {
                    return true;
                }

                return (data.Title.toLowerCase().indexOf(this.pageFilterText.toLowerCase()) > -1);
            },
            articlesFilterByTitle: function (data) {
                // no search, don't filter :
                if (this.articleFilterText.length === 0) {
                    return true;
                }

                return (data.Title.toLowerCase().indexOf(this.articleFilterText.toLowerCase()) > -1);
            },
            categoriesFilterByName: function (data) {
                // no search, don't filter :
                if (this.categoryFilterText.length === 0) {
                    return true;
                }

                return (data.Name.toLowerCase().indexOf(this.categoryFilterText.toLowerCase()) > -1);
            },
        },
        computed: {
            filteredPage: function () {
                return this.pages
                    .filter(this.pagesFilterByTitle);
            },
            filteredArticle: function () {
                return this.articles
                    .filter(this.articlesFilterByTitle);
            },
            filteredCategory: function () {
                return this.categories
                    .filter(this.categoriesFilterByName);
            }
        },
        mounted() {
            this.getAllMenus();
            this.getAllPages();
            this.getAllArticles();
            this.getAllCategories();
        }
    }
</script>

<style lang="scss">
    @import '@core/scss/vue/libs/vue-select.scss';

    .menu-banner {
        background: #ffffff !important;
        padding: 10px;
        border-radius: 5px;
        -webkit-box-shadow: 0px 0px 3px 0px rgba(196,196,196,1);
        -moz-box-shadow: 0px 0px 3px 0px rgba(196,196,196,1);
        box-shadow: 0px 0px 3px 0px rgba(196,196,196,1);
    }

    .menu-banner .menu-select {
        padding: 0;
    }

    #menu-items .card-body {
        padding: 0 !important;
    }

    #menu-items .menu-item-search {
        margin: 5px 10px 0 10px;
    }

    #menu-items .menu-item-list {
        margin: 10px;
        padding: 10px;
        -webkit-box-shadow: inset 0px 0px 1.5px 0px rgba(0,0,0,0.75);
        -moz-box-shadow: inset 0px 0px 1.5px 0px rgba(0,0,0,0.75);
        box-shadow: inset 0px 0px 1.5px 0px rgba(0,0,0,0.75);
        border-radius: 5px;
    }

    #menu-items .menu-items-action, #menu-items .custom-menu-item {
        margin: 10px;
    }

    #menu-items .menu-items-action .custom-checkbox {
        display: inline-block !important;
        margin-top: 5px;
    }

    .card-header {
        padding: 8px !important;
        border-bottom: 1px solid #ebe9f1 !important;
    }

    .dragArea {
        min-height: 50px;
        outline: 1px dashed;
    }

    .list-group-item {
        transition: all 1s
    }
</style>