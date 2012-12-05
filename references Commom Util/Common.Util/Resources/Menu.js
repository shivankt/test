﻿var allUIMenus = []; $.fn.menu = function (C) { var A = this; var C = C; var B = new Menu(A, C); allUIMenus.push(B); $(this).mousedown(function () { if (!B.menuOpen) { B.showLoading() } }).mouseover(function () { if (B.menuOpen == false) { B.showMenu() } else { B.kill() } return false }) }; function Menu(B, E) { var C = this; var B = $(B); var D = $('<div class="fg-menu-container ui-widget ui-widget-content ui-corner-all">' + E.content + "</div>"); this.menuOpen = false; this.menuExists = false; var E = jQuery.extend({ content: null, width: 180, maxHeight: 180, positionOpts: { posX: "left", posY: "bottom", offsetX: 0, offsetY: 0, directionH: "right", directionV: "down", detectH: true, detectV: true, linkToFront: false }, showSpeed: 200, callerOnState: "ui-state-active", loadingState: "ui-state-loading", linkHover: "ui-state-hover", linkHoverSecondary: "li-hover", crossSpeed: 200, crumbDefaultText: "Choose an option:", backLink: true, backLinkText: "Back", flyOut: false, flyOutOnState: "ui-state-default", nextMenuLink: "ui-icon-triangle-1-e", topLinkText: "All", nextCrumbLink: "ui-icon-carat-1-e" }, E); var A = function () { $.each(allUIMenus, function (F) { if (allUIMenus[F].menuOpen) { allUIMenus[F].kill() } }) }; this.kill = function () { B.removeClass(E.loadingState).removeClass("fg-menu-open").removeClass(E.callerOnState); D.find("li").removeClass(E.linkHoverSecondary).find("a").removeClass(E.linkHover); if (E.flyOutOnState) { D.find("li a").removeClass(E.flyOutOnState) } if (E.callerOnState) { B.removeClass(E.callerOnState) } if (D.is(".fg-menu-ipod")) { C.resetDrilldownMenu() } if (D.is(".fg-menu-flyout")) { C.resetFlyoutMenu() } D.parent().hide(); C.menuOpen = false; $(document).unbind("click", A); $(document).unbind("keydown") }; this.showLoading = function () { B.addClass(E.loadingState) }; this.showMenu = function () { A(); if (!C.menuExists) { C.create() } B.addClass("fg-menu-open").addClass(E.callerOnState); D.parent().show().click(function () { C.kill(); return false }); D.hide().slideDown(E.showSpeed).find(".fg-menu:eq(0)"); C.menuOpen = true; B.removeClass(E.loadingState); $(document).click(A); $(document).keydown(function (G) { var J; if (G.which != "") { J = G.which } else { if (G.charCode != "") { J = G.charCode } else { if (G.keyCode != "") { J = G.keyCode } } } var F = ($(G.target).parents("div").is(".fg-menu-flyout")) ? "flyout" : "ipod"; switch (J) { case 37: if (F == "flyout") { $(G.target).trigger("mouseout"); if ($("." + E.flyOutOnState).size() > 0) { $("." + E.flyOutOnState).trigger("mouseover") } } if (F == "ipod") { $(G.target).trigger("mouseout"); if ($(".fg-menu-footer").find("a").size() > 0) { $(".fg-menu-footer").find("a").trigger("click") } if ($(".fg-menu-header").find("a").size() > 0) { $(".fg-menu-current-crumb").prev().find("a").trigger("click") } if ($(".fg-menu-current").prev().is(".fg-menu-indicator")) { $(".fg-menu-current").prev().trigger("mouseover") } } return false; break; case 38: if ($(G.target).is("." + E.linkHover)) { var H = $(G.target).parent().prev().find("a:eq(0)"); if (H.size() > 0) { $(G.target).trigger("mouseout"); H.trigger("mouseover") } } else { D.find("a:eq(0)").trigger("mouseover") } return false; break; case 39: if ($(G.target).is(".fg-menu-indicator")) { if (F == "flyout") { $(G.target).next().find("a:eq(0)").trigger("mouseover") } else { if (F == "ipod") { $(G.target).trigger("click"); setTimeout(function () { $(G.target).next().find("a:eq(0)").trigger("mouseover") }, E.crossSpeed) } } } return false; break; case 40: if ($(G.target).is("." + E.linkHover)) { var I = $(G.target).parent().next().find("a:eq(0)"); if (I.size() > 0) { $(G.target).trigger("mouseout"); I.trigger("mouseover") } } else { D.find("a:eq(0)").trigger("mouseover") } return false; break; case 27: A(); break; case 13: if ($(G.target).is(".fg-menu-indicator") && F == "ipod") { $(G.target).trigger("click"); setTimeout(function () { $(G.target).next().find("a:eq(0)").trigger("mouseover") }, E.crossSpeed) } break } }) }; this.create = function () { D.css({ width: E.width }).appendTo("body").find("ul:first").not(".fg-menu-breadcrumb").addClass("fg-menu"); D.find("ul, li a").addClass("ui-corner-all"); D.find("ul").attr("role", "menu").eq(0).attr("aria-activedescendant", "active-menuitem").attr("aria-labelledby", B.attr("id")); D.find("li").attr("role", "menuitem"); D.find("li:has(ul)").attr("aria-haspopup", "true").find("ul").attr("aria-expanded", "false"); D.find("a").attr("tabindex", "-1"); if (D.find("ul").size() > 1) { if (E.flyOut) { C.flyout(D, E) } else { C.drilldown(D, E) } } else { D.find("a").click(function () { C.chooseItem(this); return false }) } if (E.linkHover) { var F = D.find(".fg-menu li a"); F.hover(function () { var G = $(this); $("." + E.linkHover).removeClass(E.linkHover).blur().parent().removeAttr("id"); $(this).addClass(E.linkHover).focus().parent().attr("id", "active-menuitem") }, function () { $(this).removeClass(E.linkHover).blur().parent().removeAttr("id") }) } if (E.linkHoverSecondary) { D.find(".fg-menu li").hover(function () { $(this).siblings("li").removeClass(E.linkHoverSecondary); if (E.flyOutOnState) { $(this).siblings("li").find("a").removeClass(E.flyOutOnState) } $(this).addClass(E.linkHoverSecondary) }, function () { $(this).removeClass(E.linkHoverSecondary) }) } C.setPosition(D, B, E); C.menuExists = true }; this.chooseItem = function (F) { C.kill(); location.href = $(F).attr("href") } } Menu.prototype.flyout = function (B, C) { var A = this; this.resetFlyoutMenu = function () { var D = B.find("ul ul"); D.removeClass("ui-widget-content").hide() }; B.addClass("fg-menu-flyout").find("li:has(ul)").each(function () { var G = B.width(); var F, D; var E = $(this).find("ul"); E.css({ left: G, width: G }).hide(); $(this).find("a:eq(0)").addClass("fg-menu-indicator").html("<span>" + $(this).find("a:eq(0)").text() + '</span><span class="ui-icon ' + C.nextMenuLink + '"></span>').hover(function () { clearTimeout(D); var H = $(this).next(); if (!fitVertical(H, $(this).offset().top)) { H.css({ top: "auto", bottom: 0 }) } if (!fitHorizontal(H, $(this).offset().left + 100)) { H.css({ left: "auto", right: G, "z-index": 999 }) } F = setTimeout(function () { H.addClass("ui-widget-content").show(C.showSpeed).attr("aria-expanded", "true") }, 300) }, function () { clearTimeout(F); var H = $(this).next(); D = setTimeout(function () { H.removeClass("ui-widget-content").hide(C.showSpeed).attr("aria-expanded", "false") }, 400) }); $(this).find("ul a").hover(function () { clearTimeout(D); if ($(this).parents("ul").prev().is("a.fg-menu-indicator")) { $(this).parents("ul").prev().addClass(C.flyOutOnState) } }, function () { D = setTimeout(function () { E.hide(C.showSpeed); B.find(C.flyOutOnState).removeClass(C.flyOutOnState) }, 500) }) }); B.find("a").click(function () { A.chooseItem(this); return false }) }; Menu.prototype.drilldown = function (H, I) { var E = this; var M = H.find(".fg-menu"); var C = $('<ul class="fg-menu-breadcrumb ui-widget-header ui-corner-all ui-helper-clearfix"></ul>'); var G = $('<li class="fg-menu-breadcrumb-text">' + I.crumbDefaultText + "</li>"); var L = (I.backLink) ? I.backLinkText : I.topLinkText; var B = (I.backLink) ? "fg-menu-prev-list" : "fg-menu-all-lists"; var K = (I.backLink) ? "ui-state-default ui-corner-all" : ""; var J = (I.backLink) ? '<span class="ui-icon ui-icon-triangle-1-w"></span>' : ""; var D = $('<li class="' + B + '"><a href="#" class="' + K + '">' + J + L + "</a></li>"); H.addClass("fg-menu-ipod"); if (I.backLink) { C.addClass("fg-menu-footer").appendTo(H).hide() } else { C.addClass("fg-menu-header").prependTo(H) } C.append(G); var F = function (N) { if (N.height() > I.maxHeight) { N.addClass("fg-menu-scroll") } N.css({ height: I.maxHeight }) }; var A = function (N) { N.removeClass("fg-menu-scroll").removeClass("fg-menu-current").height("auto") }; this.resetDrilldownMenu = function () { $(".fg-menu-current").removeClass("fg-menu-current"); M.animate({ left: 0 }, I.crossSpeed, function () { $(this).find("ul").each(function () { $(this).hide(); A($(this)) }); M.addClass("fg-menu-current") }); $(".fg-menu-all-lists").find("span").remove(); C.empty().append(G); $(".fg-menu-footer").empty().hide(); F(M) }; M.addClass("fg-menu-content fg-menu-current ui-widget-content ui-helper-clearfix").css({ width: H.width() }).find("ul").css({ width: H.width(), left: H.width() }).addClass("ui-widget-content").hide(); F(M); M.find("a").each(function () { if ($(this).next().is("ul")) { $(this).addClass("fg-menu-indicator").each(function () { $(this).html("<span>" + $(this).text() + '</span><span class="ui-icon ' + I.nextMenuLink + '"></span>') }).click(function () { var O = $(this).next(); var S = $(this).parents("ul:eq(0)"); var Q = (S.is(".fg-menu-content")) ? 0 : parseFloat(M.css("left")); var R = Math.round(Q - parseFloat(H.width())); var N = $(".fg-menu-footer"); A(S); F(O); M.animate({ left: R }, I.crossSpeed); O.show().addClass("fg-menu-current").attr("aria-expanded", "true"); var P = function (V) { var W = V; var X = $(".fg-menu-current"); var Y = X.parents("ul:eq(0)"); X.hide().attr("aria-expanded", "false"); A(X); F(Y); Y.addClass("fg-menu-current").attr("aria-expanded", "true"); if (Y.hasClass("fg-menu-content")) { W.remove(); N.hide() } }; if (I.backLink) { if (N.find("a").size() == 0) { N.show(); $('<a href="#"><span class="ui-icon ui-icon-triangle-1-w"></span> <span>Back</span></a>').appendTo(N).click(function () { var W = $(this); var V = parseFloat(M.css("left")) + H.width(); M.animate({ left: V }, I.crossSpeed, function () { P(W) }); return false }) } } else { if (C.find("li").size() == 1) { C.empty().append(D); D.find("a").click(function () { E.resetDrilldownMenu(); return false }) } $(".fg-menu-current-crumb").removeClass("fg-menu-current-crumb"); var T = $(this).find("span:eq(0)").text(); var U = $('<li class="fg-menu-current-crumb"><a href="javascript://" class="fg-menu-crumb">' + T + "</a></li>"); U.appendTo(C).find("a").click(function () { if ($(this).parent().is(".fg-menu-current-crumb")) { E.chooseItem(this) } else { var V = -($(".fg-menu-current").parents("ul").size() - 1) * 180; M.animate({ left: V }, I.crossSpeed, function () { P() }); $(this).parent().addClass("fg-menu-current-crumb").find("span").remove(); $(this).parent().nextAll().remove() } return false }); U.prev().append(' <span class="ui-icon ' + I.nextCrumbLink + '"></span>') } return false }) } else { $(this).click(function () { E.chooseItem(this); return false }) } }) }; Menu.prototype.setPosition = function (I, E, B) { var G = I; var D = E; var F = { refX: D.offset().left, refY: D.offset().top, refW: D.getTotalWidth(), refH: D.getTotalHeight() }; var B = B; var A, C; var H = $('<div class="positionHelper"></div>'); H.css({ position: "absolute", left: F.refX, top: F.refY, width: F.refW, height: F.refH }); G.wrap(H); switch (B.positionOpts.posX) { case "left": A = 0; break; case "center": A = F.refW / 2; break; case "right": A = F.refW; break } switch (B.positionOpts.posY) { case "top": C = 0; break; case "center": C = F.refH / 2; break; case "bottom": C = F.refH; break } A += B.positionOpts.offsetX; C += B.positionOpts.offsetY; if (B.positionOpts.directionV == "up") { G.css({ top: "auto", bottom: C }); if (B.positionOpts.detectV && !fitVertical(G)) { G.css({ bottom: "auto", top: C }) } } else { G.css({ bottom: "auto", top: C }); if (B.positionOpts.detectV && !fitVertical(G)) { G.css({ top: "auto", bottom: C }) } } if (B.positionOpts.directionH == "left") { G.css({ left: "auto", right: A }); if (B.positionOpts.detectH && !fitHorizontal(G)) { G.css({ right: "auto", left: A }) } } else { G.css({ right: "auto", left: A }); if (B.positionOpts.detectH && !fitHorizontal(G)) { G.css({ left: "auto", right: A }) } } if (B.positionOpts.linkToFront) { D.clone().addClass("linkClone").css({ position: "absolute", top: 0, right: "auto", bottom: "auto", left: 0, width: D.width(), height: D.height() }).insertAfter(G) } }; function sortBigToSmall(A, B) { return B - A } jQuery.fn.getTotalWidth = function () { return $(this).width() + parseInt($(this).css("paddingRight")) + parseInt($(this).css("paddingLeft")) + parseInt($(this).css("borderRightWidth")) + parseInt($(this).css("borderLeftWidth")) }; jQuery.fn.getTotalHeight = function () { return $(this).height() + parseInt($(this).css("paddingTop")) + parseInt($(this).css("paddingBottom")) + parseInt($(this).css("borderTopWidth")) + parseInt($(this).css("borderBottomWidth")) }; function getScrollTop() { return self.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop } function getScrollLeft() { return self.pageXOffset || document.documentElement.scrollLeft || document.body.scrollLeft } function getWindowHeight() { var A = document.documentElement; return self.innerHeight || (A && A.clientHeight) || document.body.clientHeight } function getWindowWidth() { var A = document.documentElement; return self.innerWidth || (A && A.clientWidth) || document.body.clientWidth } function fitHorizontal(C, B) { var A = parseInt(B) || $(C).offset().left; return (A + $(C).width() <= getWindowWidth() + getScrollLeft() && A - getScrollLeft() >= 0) } function fitVertical(B, C) { var A = parseInt(C) || $(B).offset().top; return (A + $(B).height() <= getWindowHeight() + getScrollTop() && A - getScrollTop() >= 0) } Number.prototype.pxToEm = String.prototype.pxToEm = function (C) { C = jQuery.extend({ scope: "body", reverse: false }, C); var A = (this == "") ? 0 : parseFloat(this); var E; var F = function () { var G = document.documentElement; return self.innerWidth || (G && G.clientWidth) || document.body.clientWidth }; if (C.scope == "body" && $.browser.msie && (parseFloat($("body").css("font-size")) / F()).toFixed(1) > 0) { var D = function () { return (parseFloat($("body").css("font-size")) / F()).toFixed(3) * 16 }; E = D() } else { E = parseFloat(jQuery(C.scope).css("font-size")) } var B = (C.reverse == true) ? (A * E).toFixed(2) + "px" : (A / E).toFixed(2) + "em"; return B };
$(function () {
    $(".MenuItemsHorizontal>li.LiSubMenu").each(function () {
        var ulId = "#" + $(this).attr("rel");
        $(this).menu({ content: '<ul>' + $(ulId).html() + '</ul>', flyOut: true });
    });
});