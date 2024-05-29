/***************************************************************************************************
 * BROWSER POLYFILLS
 */

// Polyfills para suporte ao IE9, IE10 e IE11
// import 'core-js/es6/symbol';
// import 'core-js/es6/object';
// import 'core-js/es6/function';
// import 'core-js/es6/parse-int';
// import 'core-js/es6/parse-float';
// import 'core-js/es6/number';
// import 'core-js/es6/math';
// import 'core-js/es6/string';
// import 'core-js/es6/date';
// import 'core-js/es6/array';
// import 'core-js/es6/regexp';
// import 'core-js/es6/map';
// import 'core-js/es6/set';

/**
 * Polyfills para navegadores evergreen.
 */
import "core-js/es/reflect";

/**
 * Web Animations `@angular/platform-browser/animations`
 * Somente necessário se AnimationBuilder for usado e utilizando IE/Edge ou Safari.
 * O suporte padrão a animações no Angular NÃO requer polyfills.
 **/
// import 'web-animations-js';  // Execute `npm install --save web-animations-js`.

/***************************************************************************************************
 * Zone JS é necessário para o Angular.
 */
import "zone.js"; // Incluído com o Angular CLI.

/***************************************************************************************************
 * APLICAÇÃO IMPORTS
 */

/**
 * Pipes de Data, moeda, decimal e percent.
 * Necessário para: Todos os navegadores, exceto Chrome, Firefox, Edge, IE11 e Safari 10
 */
// import 'intl';  // Execute `npm install --save intl`.
