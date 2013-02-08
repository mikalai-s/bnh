(function($) {
	/**
	 *
	 * RoyalSlider auto height module
	 * @version 1.0.2:
	 *
	 * 1.0.2
	 * - Changed "on" to "one" in afterInit event listener
	 * - Removed id="clear"
	 */ 
	$.extend($.rsProto, {
		_initAutoHeight: function() {
			var self = this;
			if(self.st.autoHeight) {
				var holder,
					tH,
					currSlide,
					currHeight,
					updHeight = function(animate) {
						currSlide = self.slides[self.currSlideId];
						holder = currSlide.holder;

						if(holder) {
							tH = holder.height();
							if(tH && tH !== currHeight) {
								self._wrapHeight = tH;
								if(self._useCSS3Transitions || !animate) {
									self._sliderOverflow.css('height', tH);
								} else {
									self._sliderOverflow.stop(true,true).animate({height: tH}, self.st.transitionSpeed);
								}
							}
						}
					};

				self.ev.on('rsMaybeSizeReady.rsAutoHeight', function(e, slideObject) {
					if(currSlide === slideObject) {
						updHeight();
					}
				});

				self.ev.on('rsAfterContentSet.rsAutoHeight', function(e, slideObject) {
					if(currSlide === slideObject) {
						updHeight();
					}
				});
				


				self.slider.addClass('rsAutoHeight');
				self.ev.one('rsAfterInit', function() {
					setTimeout(function() {
						updHeight(false);
						setTimeout(function() {
							self.slider.append('<div style="clear:both; float: none;"></div>');
							if(self._useCSS3Transitions) {
								self._sliderOverflow.css(self._vendorPref + 'transition', 'height ' + self.st.transitionSpeed + 'ms ease-in-out');
							}
						}, 16);
					}, 16);
				});
				self.ev.on('rsBeforeAnimStart', function() {
					updHeight(true);
				});
				self.ev.on('rsBeforeSizeSet' , function() {
					setTimeout(function() {
						updHeight(false);
					}, 16);
				});
			}
			
		}
	});
	$.rsModules.autoHeight = $.rsProto._initAutoHeight;
})(jQuery);