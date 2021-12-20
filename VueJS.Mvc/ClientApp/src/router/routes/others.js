export default [
  {
    path: '/access-control',
    name: 'access-control',
    component: () => import('@/views/admin/extensions/acl/AccessControl.vue'),
    meta: {
      resource: 'ACL',
      action: 'read',
    },
  },
]
