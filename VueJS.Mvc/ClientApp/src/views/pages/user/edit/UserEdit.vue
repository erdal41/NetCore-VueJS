<template>
    <b-row>
        <b-col class="content-header-left mb-2"
               cols="12"
               md="8">
            <b-row class="breadcrumbs-top">
                <b-col cols="12">
                    <h2 class="content-header-title float-left pr-1 mb-0">
                        Kullanıcı Düzenle
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
               md="4"
               cols="12">
            <b-button v-if="$can('create', 'User')"
                      variant="primary"
                      type="button"
                      :to="{name: 'pages-user-add'}">
                Yeni Ekle
            </b-button>
        </b-col>
        <b-col lg="12"
               md="12">
            <b-tabs vertical
                    content-class="col-12 col-md-9 mt-1 mt-md-0"
                    pills
                    nav-wrapper-class="col-md-3 col-12"
                    nav-class="nav-left">

                <!-- general tab -->
                <b-tab active>

                    <!-- title -->
                    <template #title>
                        <feather-icon icon="UserIcon"
                                      size="18"
                                      class="mr-50" />
                        <span class="font-weight-bold">Genel</span>
                    </template>

                    <user-edit-general v-if="users.UserUpdateDto"
                                       :general-data="users.UserUpdateDto" />
                </b-tab>
                <!--/ general tab -->
                <!-- change password tab -->
                <b-tab>

                    <!-- title -->
                    <template #title>
                        <feather-icon icon="LockIcon"
                                      size="18"
                                      class="mr-50" />
                        <span class="font-weight-bold">Şifre Değiştir</span>
                    </template>

                    <user-edit-password />
                </b-tab>
                <!--/ change password tab -->
                <!-- info -->
                <b-tab>

                    <!-- title -->
                    <template #title>
                        <feather-icon icon="InfoIcon"
                                      size="18"
                                      class="mr-50" />
                        <span class="font-weight-bold">Kişisel Bilgiler</span>
                    </template>

                    <user-edit-information v-if="users.UserUpdateDto"
                                           :information-data="users.UserUpdateDto" />
                </b-tab>

                <!-- social links -->
                <b-tab>

                    <!-- title -->
                    <template #title>
                        <feather-icon icon="LinkIcon"
                                      size="18"
                                      class="mr-50" />
                        <span class="font-weight-bold">Sosyal Medya</span>
                    </template>

                    <user-edit-social v-if="users.UserUpdateDto"
                                      :social-data="users.UserUpdateDto" />
                </b-tab>

                <!-- roles -->
                <b-tab>

                    <!-- title -->
                    <template #title>
                        <feather-icon icon="ShieldIcon"
                                      size="18"
                                      class="mr-50" />
                        <span class="font-weight-bold">Roller</span>
                    </template>

                    <user-edit-role v-if="users.UserUpdateDto"
                                    :role-data="users.UserRolesViewModel" />
                </b-tab>                
            </b-tabs>
        </b-col>
    </b-row>
</template>

<script>
    import { BRow, BCol, BBreadcrumb, BBreadcrumbItem, BButton, BTabs, BTab } from 'bootstrap-vue'
    import UserEditGeneral from './UserEditGeneral.vue'
    import UserEditPassword from './UserEditPassword.vue'
    import UserEditInformation from './UserEditInformation.vue'
    import UserEditSocial from './UserEditSocial.vue'
    import UserEditRole from './UserEditRole.vue'

    export default {
        components: {
            BRow,
            BCol,
            BBreadcrumb,
            BBreadcrumbItem,
            BButton,
            BTabs,
            BTab,
            UserEditGeneral,
            UserEditPassword,
            UserEditInformation,
            UserEditSocial,
            UserEditRole,
        },
        data() {
            return {
                breadcrumbs: [
                    {
                        text: 'Kullanıcılar',
                        to: { name: 'pages-user-list' },
                    },
                    {
                        text: 'Düzenle',
                        active: true,
                    }
                ],
                options: {},
                users: {}
            }
        },
        beforeCreate() {
            this.$http.get('/account-setting/data').then(res => {
                console.log(res.data)
                this.options = res.data
            })

            this.$http.get('/admin/user-edit?userId=' + this.$route.query.edit).then(res => {
                console.log(res.data)
                this.users = res.data
            }).catch((error) => {
                console.log(error)
                console.log(error.request)
            })
        },
    }
</script>
