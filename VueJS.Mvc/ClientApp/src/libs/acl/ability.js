import { Ability } from '@casl/ability'
import { initialAbility } from './config'

//  Read ability from localStorage
// * Handles auto fetching previous abilities if already logged in user
// ? You can update this if you store user abilities to more secure place
// ! Anyone can update localStorage so be careful and please update this
const userData = JSON.parse(localStorage.getItem('userData'))
const ability = new Array();
console.log("default");
console.log(ability);
ability.push({ subject: 'Auth', action: 'read' })
console.log("userData");
console.log(userData);
if (userData != null) {
    for (let role of userData.Roles) {
        ability.push({ subject: role.split(".")[0], action: role.split(".")[1] });
    }
}

console.log("roles");
console.log(ability);



const existingAbility = userData ? ability : null

export default new Ability(existingAbility || initialAbility)
