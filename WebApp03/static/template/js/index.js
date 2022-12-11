/**
 Alan, for Front-index
 */
window.onload = function() {
	fetch('/CGA102G3/ProdServlet.do?action=indexshop')
		.then(response => response.json())
		.then(function(myjson) {
			for (let i = 0; i < myjson.length; i++) {
				let json = myjson[i];
				
			// 	if (json.discount === 'Y') {
			// 		$('#prod_list').append(`</li>
         	// 	 <div class="col-3" style="margin-top:20px; margin-bottom:20px">
         	// 	 	<div class="mb-2" style="height: 200px;margin: auto;text-align: center;">
         	// 	 		<img style="height:100%; margin:auto;" src="/CGA102G3/static/images/books/${json.bookID}.jpg"></div>
            //      	<div class="title" style="width:200px; font-weight:700;">
            //      		<a href="/CGA102G3/ProdServlet.do?action=getOne_For_Display&prodID=${json.prodID}" style="width:100px" >${json.title}</a></div>
            //   	 	<div class="2-price text-dark mb-2 p-2" style="width:200px;">
            //   	 		定價: <span style="text-decoration:line-through;">${json.price1}</span>元<br>
            //   	 		優惠: <span style="color:indianred;">${json.price2}</span>元</div>
          	// 	 	<div class="2-btn"style="width:200px;display:flex;margin-left:-15px">
          	// 	 		<button class="btn btn-sm btn-warning ml-3" style="width:100px" onClick="add(${json.bookID})">
          	// 	 		放入購物車</button>
           	// 	 </div></li>`)
			// 	} else {
			// 		$('#prod_list').append(`</li>
         	// 	 <div class="col-3" style="margin-top:20px; margin-bottom:20px">
         	// 	 	<div class="mb-2" style="height: 200px;margin: auto;text-align: center;">
         	// 	 		<img style="height:100%; margin:auto;" src="/CGA102G3/static/images/books/${json.bookID}.jpg"></div>
            //      	<div class="title" style="width:200px; font-weight:700;">
            //      		<a href="/CGA102G3/ProdServlet.do?action=getOne_For_Display&prodID=${json.prodID}" style="width:100px" >${json.title}</a></div>
            //   	 	<div class="2-price text-dark mb-2 p-2" style="width:200px;">
            //   	 		定價: <span>${json.price1}</span>元</div><br>
          	// 	 	<div class="2-btn"style="width:200px;display:flex;margin-left:-15px">
          	// 	 		<button class="btn btn-sm btn-warning ml-3" style="width:100px" onClick="add(${json.bookID})">
          	// 	 		放入購物車</button>
			// 		</div></li>`)
			// 	}
			// }
				let x = 0;
				let json2= myjson[i+1];
				if(json2){
					x = json2.prodID
				}
				let y = 0;
				let json3= myjson[i-1];
				if(json3){
					y = json3.prodID;
				}
				if (json.prodID  === x) {
							$('#prod_list').append(`</li>
						 <div class="col-3" style="margin-top:20px; margin-bottom:20px">
						 	<div class="mb-2" style="height: 200px;margin: auto;text-align: center;">
						 		<img style="height:100%; margin:auto;" src="/CGA102G3/static/images/books/${json2.bookID}.jpg"></div>
					     	<div class="title" style="width:200px; font-weight:700;">
					     		<a href="/CGA102G3/ProdServlet.do?action=getOne_For_Display&prodID=${json2.prodID}" style="width:100px" >${json2.title}</a></div>
					  	 	<div class="2-price text-dark mb-2 p-2" style="width:200px;">
					  	 		定價: <span style="text-decoration:line-through;">${json2.price1}</span>元<br>
					  	 		優惠: <span style="color:indianred;">${json2.price2}</span>元</div>
						 	<div class="2-btn"style="width:200px;display:flex;margin-left:-15px">
						 		<button class="btn btn-sm btn-warning ml-3" style="width:100px" onClick="add(${json2.bookID})">
						 		放入購物車</button>
						 </div></li>`)
					} else if (json.prodID === y) {
					continue;
				}
				else {
					if (json.discount === 'Y') {
						$('#prod_list').append(`</li>
						 <div class="col-3" style="margin-top:20px; margin-bottom:20px">
						 	<div class="mb-2" style="height: 200px;margin: auto;text-align: center;">
						 		<img style="height:100%; margin:auto;" src="/CGA102G3/static/images/books/${json.bookID}.jpg"></div>
					     	<div class="title" style="width:200px; font-weight:700;">
					     		<a href="/CGA102G3/ProdServlet.do?action=getOne_For_Display&prodID=${json.prodID}" style="width:100px" >${json.title}</a></div>
					  	 	<div class="2-price text-dark mb-2 p-2" style="width:200px;">
					  	 		定價: <span style="text-decoration:line-through;">${json.price1}</span>元<br>
					  	 		優惠: <span style="color:indianred;">${json.price2}</span>元</div>
						 	<div class="2-btn"style="width:200px;display:flex;margin-left:-15px">
						 		<button class="btn btn-sm btn-warning ml-3" style="width:100px" onClick="add(${json.bookID})">
						 		放入購物車</button>
						 </div></li>`)
					}  else if (json.discount === 'N') {
								$('#prod_list').append(`</li>
							 <div class="col-3" style="margin-top:20px; margin-bottom:20px">
							 	<div class="mb-2" style="height: 200px;margin: auto;text-align: center;">
							 		<img style="height:100%; margin:auto;" src="/CGA102G3/static/images/books/${json.bookID}.jpg"></div>
						     	<div class="title" style="width:200px; font-weight:700;">
						     		<a href="/CGA102G3/ProdServlet.do?action=getOne_For_Display&prodID=${json.prodID}" style="width:100px" >${json.title}</a></div>
						  	 	<div class="2-price text-dark mb-2 p-2" style="width:200px;">
						  	 		定價: <span>${json.price1}</span>元</div><br>
							 	<div class="2-btn"style="width:200px;display:flex;margin-left:-15px">
							 		<button class="btn btn-sm btn-warning ml-3" style="width:100px" onClick="add(${json.bookID})">
							 		放入購物車</button>
								</div></li>`)
							}
				}
			}
		})


	fetch('/CGA102G3/ProdServlet.do?action=indexts')
		.then(response => response.json())
		.then(function(myjson) {
			console.log(myjson);
			for (let i = 0; i < myjson.length; i++) {
				let json = myjson[i];

				// if (json.discount === 'Y') {
				// 	$('#topsale_list').append(`</li>
         		//  <div class="col-3" style="margin-top:20px; margin-bottom:20px">
         		//  	<div class="mb-2" style="height: 200px;margin: auto;text-align: center;">
         		//  		<img style="height:100%; margin:auto;" src="/CGA102G3/static/images/books/${json.bookID}.jpg"></div>
                //  	<div class="title" style="width:200px; font-weight:700;">
                //  		<a href="/CGA102G3/ProdServlet.do?action=getOne_For_Display&prodID=${json.prodID}" style="width:100px" >${json.title}</a></div>
              	//  	<div class="2-price text-dark mb-2 p-2" style="width:200px;">
              	//  		定價: <span style="text-decoration:line-through;">${json.price1}</span>元<br>
              	//  		優惠: <span style="color:indianred;">${json.price2}</span>元</div>
          		//  	<div class="2-btn"style="width:200px;display:flex;margin-left:-15px">
          		//  		<button class="btn btn-sm btn-warning ml-3" style="width:100px" onClick="add(${json.bookID})">
          		//  		放入購物車</button>
           		//  </div></li>`)
				// } else {
				// 	$('#topsale_list').append(`</li>
         		//  <div class="col-3" style="margin-top:20px; margin-bottom:20px">
         		//  	<div class="mb-2" style="height: 200px;margin: auto;text-align: center;">
         		//  		<img style="height:100%; margin:auto;" src="/CGA102G3/static/images/books/${json.bookID}.jpg"></div>
                //  	<div class="title" style="width:200px; font-weight:700;">
                //  		<a href="/CGA102G3/ProdServlet.do?action=getOne_For_Display&prodID=${json.prodID}" style="width:100px" >${json.title}</a></div>
              	//  	<div class="2-price text-dark mb-2 p-2" style="width:200px;">
              	//  		定價: <span>${json.price1}</span>元</div><br>
          		//  	<div class="2-btn"style="width:200px;display:flex;margin-left:-15px">
          		//  		<button class="btn btn-sm btn-warning ml-3" style="width:100px" onClick="add(${json.bookID})">
          		//  		放入購物車</button>
          		//  		</div></li>`)
				// }

				let x = 0;
				let json2= myjson[i+1];
				if(json2){
					x = json2.prodID
				}
				let y = 0;
				let json3= myjson[i-1];
				if(json3){
					y = json3.prodID;
				}
				if (json.prodID  === x) {
					$('#topsale_list').append(`</li>
						 <div class="col-3" style="margin-top:20px; margin-bottom:20px">
						 	<div class="mb-2" style="height: 200px;margin: auto;text-align: center;">
						 		<img style="height:100%; margin:auto;" src="/CGA102G3/static/images/books/${json2.bookID}.jpg"></div>
					     	<div class="title" style="width:200px; font-weight:700;">
					     		<a href="/CGA102G3/ProdServlet.do?action=getOne_For_Display&prodID=${json2.prodID}" style="width:100px" >${json2.title}</a></div>
					  	 	<div class="2-price text-dark mb-2 p-2" style="width:200px;">
					  	 		定價: <span style="text-decoration:line-through;">${json2.price1}</span>元<br>
					  	 		優惠: <span style="color:indianred;">${json2.price2}</span>元</div>
						 	<div class="2-btn"style="width:200px;display:flex;margin-left:-15px">
						 		<button class="btn btn-sm btn-warning ml-3" style="width:100px" onClick="add(${json2.bookID})">
						 		放入購物車</button>
						 </div></li>`)
				} else if (json.prodID === y) {
					continue;
				}
				else {
					if (json.discount === 'Y') {
						$('#topsale_list').append(`</li>
						 <div class="col-3" style="margin-top:20px; margin-bottom:20px">
						 	<div class="mb-2" style="height: 200px;margin: auto;text-align: center;">
						 		<img style="height:100%; margin:auto;" src="/CGA102G3/static/images/books/${json.bookID}.jpg"></div>
					     	<div class="title" style="width:200px; font-weight:700;">
					     		<a href="/CGA102G3/ProdServlet.do?action=getOne_For_Display&prodID=${json.prodID}" style="width:100px" >${json.title}</a></div>
					  	 	<div class="2-price text-dark mb-2 p-2" style="width:200px;">
					  	 		定價: <span style="text-decoration:line-through;">${json.price1}</span>元<br>
					  	 		優惠: <span style="color:indianred;">${json.price2}</span>元</div>
						 	<div class="2-btn"style="width:200px;display:flex;margin-left:-15px">
						 		<button class="btn btn-sm btn-warning ml-3" style="width:100px" onClick="add(${json.bookID})">
						 		放入購物車</button>
						 </div></li>`)
					}  else if (json.discount === 'N') {
						$('#topsale_list').append(`</li>
							 <div class="col-3" style="margin-top:20px; margin-bottom:20px">
							 	<div class="mb-2" style="height: 200px;margin: auto;text-align: center;">
							 		<img style="height:100%; margin:auto;" src="/CGA102G3/static/images/books/${json.bookID}.jpg"></div>
						     	<div class="title" style="width:200px; font-weight:700;">
						     		<a href="/CGA102G3/ProdServlet.do?action=getOne_For_Display&prodID=${json.prodID}" style="width:100px" >${json.title}</a></div>
						  	 	<div class="2-price text-dark mb-2 p-2" style="width:200px;">
						  	 		定價: <span>${json.price1}</span>元</div><br>
							 	<div class="2-btn"style="width:200px;display:flex;margin-left:-15px">
							 		<button class="btn btn-sm btn-warning ml-3" style="width:100px" onClick="add(${json.bookID})">
							 		放入購物車</button>
								</div></li>`)
					}
				}
			}
		})
		
			fetch('/CGA102G3/bid/api/getAllBidInfo')
		.then(response => response.json())
		.then(function(myjson) {
			const { data } = myjson;
			for (let i = 0; i < data.length; i++) {

				let json = data[i];
				$('#bid_list').append(` <li>       
            <div class="col-3" style="margin-top:20px">
               <div class="picture mb-2"><img src="/CGA102G3/static/images/books/${json.book_id}.jpg"></div>
                  <div style="width:200px; font-weight:700;">
                     <a href="http://localhost:8081/CGA102G3/bid/participate/getOneBid?bidID=${json.bid_id}" style="width:100px">${json.title}</a>
                  </div>
               <div class="price text-dark mb-2 p-2" style="width:200px;">
                  此刻競標價格: <span>${json.curWinner.price == -1 ? '<font color=red>未出價</font>' : json.curWinner.price}</span><br>
                  剩餘競標時間: <br><span style="color:indianred;">
                <div class="bid-time mt-2">
                    <i class="bi-alarm" style="font-size: 1.2rem; color: grey;"></i>
                    <span id="${json.bid_id}"></span>
                </div></span>
               </div>
            </div>   
          </li>`);

				// ============================競標時間倒數計時器============================
				// Set the date we're counting down to
				const countDownDate = new Date(`${json.bid_end}`).getTime();


				// Update the count down every 1 second
				const timer = setInterval(function() {

					// Get today's date and time
					const now = new Date().getTime();

					// Find the distance between now and the count down date
					const distance = countDownDate - now;

					// Time calculations for days, hours, minutes and seconds
					const days = Math.floor(distance / (1000 * 60 * 60 * 24));
					const hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
					const minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
					const seconds = Math.floor((distance % (1000 * 60)) / 1000);

					// Display the result in the element with id="demo"
					document.getElementById(`${json.bid_id}`).innerHTML = days + "天 " + hours + "時 "
						+ minutes + "分 " + seconds + "秒 ";

					// If the count down is finished, write some text
					if (distance < 0) {
						clearInterval(timer);
						document.getElementById(`${json.bid_id}`).innerHTML = "競標結束";
						const curWinnerID = $('#curWinnerID');
						if (curWinnerID.text() === '目前無會員出價') {
							curWinnerID.text('流標');
						} else {
							curWinnerID.text(curWinnerID.text() + ' 得標');
						}
					}
				}, 1000);

				// ============================競標時間倒數計時器============================

			}
		})
};


