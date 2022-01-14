export default [
    {
        path: '/admin/dashboard/analytics',
        name: 'dashboard-analytics',
        component: () => import('@/views/dashboard/analytics/Analytics.vue'),
    },
    {
        path: '/admin/dashboard/ecommerce',
        name: 'dashboard-ecommerce',
        component: () => import('@/views/dashboard/ecommerce/Ecommerce.vue'),
    },
]