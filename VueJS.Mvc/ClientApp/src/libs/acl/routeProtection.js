import ability from './ability'

export const canNavigate = to => to.matched.some(route => ability.can(route.meta.action || 'read', route.meta.resource ||'Auth' ))
console.log(ability);
export const _ = undefined
