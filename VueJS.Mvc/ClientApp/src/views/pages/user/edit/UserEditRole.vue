<template>
    <b-overlay :show="showOverlay"
               size="sm">
        <b-card>
            <b-row>
                <b-col cols="12">
                    <div class="d-flex align-items-center mb-2">
                        <feather-icon icon="ShieldIcon"
                                      size="18" />
                        <h4 class="mb-0 ml-75">
                            Kullanıcı Rolleri
                        </h4>
                    </div>
                </b-col>
            </b-row>
            <b-row>
                <b-table id="role-table"
                         striped
                         responsive
                         class="mb-0"
                         :fields="fields"
                         :items="roles"
                         ref="roleTable">
                    <template #head(read)="head">
                        <b-form-checkbox v-model="selectMultiReadCheck"
                                         :value="true"
                                         @change="selectAllReadRows">
                            {{head.label}}
                        </b-form-checkbox>
                    </template>
                    <template #head(create)="head">
                        <b-form-checkbox v-model="selectMultiCreateCheck"
                                         :value="true"
                                         @change="selectAllCreateRows">
                            {{head.label}}
                        </b-form-checkbox>
                    </template>
                    <template #head(update)="head">
                        <b-form-checkbox v-model="selectMultiUpdateCheck"
                                         :value="true"
                                         @change="selectAllUpdateRows">
                            {{head.label}}
                        </b-form-checkbox>
                    </template>
                    <template #head(delete)="head">
                        <b-form-checkbox v-model="selectMultiDeleteCheck"
                                         :value="true"
                                         @change="selectAllDeleteRows">
                            {{head.label}}
                        </b-form-checkbox>
                    </template>

                    <template #cell(module)="data">
                        {{ data.item.module }}
                    </template>
                    <template #cell(read)="data">
                        <b-form-checkbox v-show="data.value.show"
                                         v-model="checkedRoleRead"
                                         :value="data.value.action"
                                         @change="changeReadRow" />
                    </template>
                    <template #cell(create)="data">
                        <b-form-checkbox v-show="data.value.show"
                                         v-model="checkedRoleCreate"
                                         :value="data.value.action"
                                         @change="changeCreateRow" />
                    </template>
                    <template #cell(update)="data">
                        <b-form-checkbox v-show="data.value.show"
                                         v-model="checkedRoleUpdate"
                                         :value="data.value.action"
                                         @change="changeUpdateRow" />
                    </template>
                    <template #cell(delete)="data">
                        <b-form-checkbox v-show="data.value.show"
                                         v-model="checkedRoleDelete"
                                         :value="data.value.action"
                                         @change="changeDeleteRow" />
                    </template>
                </b-table>

                <b-col cols="12">
                    <hr class="my-2">
                </b-col>
                <b-col cols="12">
                    <b-spinner v-if="updateButtonDisabled"
                               variant="secondary"
                               class="align-middle mr-1" />
                    <b-button :disabled="updateButtonDisabled"
                              :variant="updateButtonVariant"
                              class="mr-1"
                              @click.prevent="roleAssign">
                        Güncelle
                    </b-button>
                    <b-button type="reset"
                              variant="outline-secondary"
                              @click.prevent="resetRole">
                        Tüm Değişiklikleri İptal Et
                    </b-button>
                </b-col>
            </b-row>
        </b-card>
    </b-overlay>
</template>

<script>
    import ToastificationContent from '@core/components/toastification/ToastificationContent.vue';
    import { BOverlay, BCard, BRow, BCol, BTable, BFormCheckboxGroup, BFormCheckbox, BSpinner, BButton } from 'bootstrap-vue'

    import axios from 'axios';

    export default {
        components: {
            ToastificationContent,
            BOverlay,
            BCard,
            BRow,
            BCol,
            BTable,
            BFormCheckboxGroup,
            BFormCheckbox,
            BSpinner,
            BButton,
        },
        props: {
            roleData: {
                type: Object,
                default: () => { },
            },
        },
        data() {
            return {
                showOverlay: false,
                localOptions: JSON.parse(JSON.stringify(this.roleData)),
                fields: [
                    { key: 'Module', label: 'Modül', sortable: false },
                    { key: 'read', label: 'Okuma', sortable: false },
                    { key: 'create', label: 'Oluşturma', sortable: false },
                    { key: 'update', label: 'Güncelleme', sortable: false },
                    { key: 'delete', label: 'SİLME', sortable: false }],
                moduleList: [
                    'Dashboard',
                    'Basepage',
                    'Otherpage',
                    'Article',
                    'Category',
                    'Tag',
                    'User',
                    'Role',
                    'Comment',
                    'Urlredirect',
                    'Seo'
                ],
                roles: [],
                userRoles: [],
                selectMultiReadCheck: false,
                selectMultiCreateCheck: false,
                selectMultiUpdateCheck: false,
                selectMultiDeleteCheck: false,
                checkedRoleRead: [],
                checkedRoleCreate: [],
                checkedRoleUpdate: [],
                checkedRoleDelete: [],
                updateButtonDisabled: false,
                updateButtonVariant: 'primary',
            }
        },
        methods: {
            getAllData() {
                this.showOverlay = true;
                if (this.localOptions) {
                    this.moduleList.forEach(module => {
                        if (module === 'Dashboard') {
                            var rolesSplit = this.localOptions.Roles.filter(role => role.Name.includes('Dashboard'));
                            this.roles.push(
                                {
                                    module: "Gösterge Paneli",
                                    read: {
                                        action: module + '.read',
                                        show: rolesSplit.filter(x => x.Name.includes('read')) ? true : false
                                    },
                                    create: {
                                        action: false,
                                        show: false
                                    },
                                    update: {
                                        action: false,
                                        show: false
                                    },
                                    delete: {
                                        action: false,
                                        show: false
                                    },
                                });
                        } else if (module === 'Role') {
                            var rolesSplit = this.localOptions.Roles.filter(role => role.Name.includes('Role'));
                            this.roles.push(
                                {
                                    module: "Roller",
                                    read: {
                                        action: false,
                                        show: false
                                    },
                                    create: {
                                        action: module + '.create',
                                        show: rolesSplit.filter(x => x.Name.includes('create')) ? true : false
                                    },
                                    update: {
                                        action: module + '.update',
                                        show: rolesSplit.filter(x => x.Name.includes('update')) ? true : false
                                    },
                                    delete: {
                                        action: false,
                                        show: false
                                    },
                                });
                        } else if (module === 'Seo') {
                            var rolesSplit = this.localOptions.Roles.filter(role => role.Name.includes('Seo'));
                            this.roles.push(
                                {
                                    module: "Seo Ayarları",
                                    read: {
                                        action: false,
                                        show: false
                                    },
                                    create: {
                                        action: module + '.create',
                                        show: rolesSplit.filter(x => x.Name.includes('create')) ? true : false
                                    },
                                    update: {
                                        action: module + '.update',
                                        show: rolesSplit.filter(x => x.Name.includes('update')) ? true : false
                                    },
                                    delete: {
                                        action: false,
                                        show: false
                                    },
                                });
                        } else {
                            var rolesSplit = this.localOptions.Roles.filter(role => role.Name.includes(module));
                            this.roles.push(
                                {
                                    module:
                                        module == 'Category' ? 'Kategori' :
                                            module == 'Basepage' ? 'Temel Sayfalar' :
                                                module == 'Otherpage' ? 'Sayfalar' :
                                                    module == 'Article' ? 'Makaleler' :
                                                        module == 'Category' ? 'Kategoriler' :
                                                            module == 'Tag' ? 'Etiketler' :
                                                                module == 'User' ? 'Kullanıcılar' :
                                                                    module == 'Comment' ? 'Yorumlar' :
                                                                        module == 'Urlredirect' ? 'Link Yönlendirme' :
                                                                            null,
                                    read: {
                                        action: module + '.read',
                                        show: rolesSplit.filter(x => x.Name.includes('read')) ? true : false
                                    },
                                    create: {
                                        action: module + '.create',
                                        show: true
                                    },
                                    update: {
                                        action: module + '.update',
                                        show: true
                                    },
                                    delete: {
                                        action: module + '.delete',
                                        show: true
                                    }
                                });
                        }
                    });

                    this.checkedRoleRead = this.localOptions.UserRoles.filter(role => role.includes('read'))
                    this.checkedRoleCreate = this.localOptions.UserRoles.filter(role => role.includes('create'))
                    this.checkedRoleUpdate = this.localOptions.UserRoles.filter(role => role.includes('update'))
                    this.checkedRoleDelete = this.localOptions.UserRoles.filter(role => role.includes('delete'))

                    this.changeReadRow();
                    this.changeCreateRow();
                    this.changeUpdateRow();
                    this.changeDeleteRow();
                    this.showOverlay = false;
                }
            },
            resetRole() {
                this.showOverlay = true;
                this.checkedRoleRead = this.localOptions.UserRoles.filter(role => role.includes('read'));
                this.checkedRoleCreate = this.localOptions.UserRoles.filter(role => role.includes('create'));
                this.checkedRoleUpdate = this.localOptions.UserRoles.filter(role => role.includes('update'));
                this.checkedRoleDelete = this.localOptions.UserRoles.filter(role => role.includes('delete'));

                this.changeReadRow();
                this.changeCreateRow();
                this.changeUpdateRow();
                this.changeDeleteRow();
                this.showOverlay = false;
            },
            changeReadRow() {
                if (this.checkedRoleRead.length == 9) {
                    this.selectMultiReadCheck = 'true';
                }
                else {
                    this.selectMultiReadCheck = 'false';
                }
            },
            changeCreateRow() {
                if (this.checkedRoleCreate.length == 10) {
                    this.selectMultiCreateCheck = 'true';
                }
                else {
                    this.selectMultiCreateCheck = 'false';
                }
            },
            changeUpdateRow() {
                if (this.checkedRoleUpdate.length == 10) {
                    this.selectMultiUpdateCheck = 'true';
                }
                else {
                    this.selectMultiUpdateCheck = 'false';
                }
            },
            changeDeleteRow() {
                if (this.checkedRoleDelete.length == 8) {
                    this.selectMultiDeleteCheck = 'true';
                }
                else {
                    this.selectMultiDeleteCheck = 'false';
                }
            },
            selectAllReadRows(value) {
                this.checkedRoleRead = [];
                if (value) {
                    var roleReads = this.localOptions.Roles.filter(role => role.Name.includes('read'));
                    roleReads.forEach(role => {
                        this.checkedRoleRead.push(role.Name);
                    })
                }
                else {
                    this.checkedRoleRead = [];
                }
            },
            selectAllCreateRows(value) {
                this.checkedRoleCreate = [];
                if (value) {
                    var roleCreates = this.localOptions.Roles.filter(role => role.Name.includes('create'));
                    roleCreates.forEach(role => {
                        this.checkedRoleCreate.push(role.Name);
                    })
                }
                else {
                    this.checkedRoleCreate = [];
                }
            },
            selectAllUpdateRows(value) {
                this.checkedRoleUpdate = [];
                if (value) {
                    var roleUpdates = this.localOptions.Roles.filter(role => role.Name.includes('update'));
                    roleUpdates.forEach(role => {
                        this.checkedRoleUpdate.push(role.Name);
                    })
                }
                else {
                    this.checkedRoleUpdate = [];
                }
            },
            selectAllDeleteRows(value) {
                this.checkedRoleDelete = [];
                if (value) {
                    var roleDeletes = this.localOptions.Roles.filter(role => role.Name.includes('delete'));
                    roleDeletes.forEach(role => {
                        this.checkedRoleDelete.push(role.Name);
                    })
                }
                else {
                    this.checkedRoleDelete = [];
                }
            },
            roleAssign() {
                this.updateButtonDisabled = true;
                this.updateButtonVariant = 'outline-secondary';
                this.userRoles = this.checkedRoleRead.concat(this.checkedRoleCreate.concat(this.checkedRoleUpdate.concat(this.checkedRoleDelete)));
                axios.post('/admin/user-roleassign',
                    {
                        UserId: this.$route.query.edit,
                        UserRoles: this.userRoles
                    }).then((response) => {
                        console.log(response.data)
                        if (response.data.UserDto.ErrorMessages === null) {
                            this.$toast({
                                component: ToastificationContent,
                                props: {
                                    variant: 'success',
                                    title: 'Başarılı İşlem!',
                                    icon: 'CheckIcon',
                                    text: response.data.UserDto.User.UserName + ' kullanıcısının rolleri güncellendi.'
                                }
                            });
                            this.updateButtonDisabled = false;
                            this.updateButtonVariant = 'primary';
                        }
                        else {
                            this.$toast({
                                component: ToastificationContent,
                                props: {
                                    variant: 'danger',
                                    title: 'Başarısız İşlem!',
                                    icon: 'AlertOctagonIcon',
                                    text: 'Roller güncellenemedi.'
                                },
                            })
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
                                text: 'Hata oluştu. Lütfen tekrar deneyin.',
                            },
                        })
                    });
            }
        },
        mounted() {
            this.getAllData();
        }
    }
</script>

<style lang="scss">
    #role-table.table th, #role-table.table td {
        padding: 0.72rem;
    }
</style>