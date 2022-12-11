const rootPath = `${window.location.origin}/`;

// ===============================添加===============================
const add_subject_submit_btn = document.querySelector(
  "#add_subject_submit_btn"
);
const addSubjectName = document.querySelector("#add_subject_name");
// ===============================添加===============================
// 搜尋
const search_btn = document.getElementById("search_btn");
// 修改
const update_subject_submit_btn = document.getElementById(
  "update_subject_submit_btn"
);

let pages = 1;
let curPage = 1;
let keyword = "";

/**
 * 加載頁面訊息
 */
function loadInfo(page, keyword) {
  $.ajax({
    type: "GET",
    url: `${rootPath}/api/subject/${page}?keyword=${keyword}`,
    dataType: "json",
    async: false,
    success: function (rsp) {
      console.log(rsp);
      const { Items, PageNo, PageTotal, TotalCounts } = rsp;
      display(Items, PageNo, PageTotal, TotalCounts);
    },
    error: function (thrownError) {
      console.log(thrownError);
    },
  });
}

/**
 * 畫面中展示員工訊息
 * @param emps: array
 */
function display(Items, PageNo, PageTotal, TotalCounts) {
  pages = PageTotal;
  curPage = PageNo;
  const tbody = $("tbody");
  tbody.empty();
  if (TotalCounts === 0) {
    tbody.append(
      `<th colspan="6" style="color: palevioletred">未找到符合條件的結果</th>>`
    );
  } else {
    for (const subject of Items) {
      tbody.append(`
      <tr class="record">
          <th scope="row" class="align-middle">${subject.Id}</th>
          <td class="align-middle">${subject.Name}</td>
          <td class="align-middle">
              <button type="button" class="btn btn-light" data-target="#update_subject_modal" data-toggle="modal">
                        <span class="update">✏</span>
               </button>
              <button type="button" class="btn btn-light">
                        <span class="delete">❌</span>
               </button>
          </td>
      </tr>
    `);
    }

    /**
     * 更新按鈕事件綁定
     * 查找所要更新數據的資訊，並將資訊寫到更新用的modal
     */
    $("span.update").click(function () {
      // 清空上次填寫欄位的值
      $("#update_form").children("input").val("");
      // 查找填入modal彈框中編號與科目名稱
      $("#update_subject_id").val(
        $(this).parents("tr.record").children("th:eq(0)").text()
      );
      $("#update_subject_name").val(
        $(this).parents("tr.record").children("td:eq(0)").text()
      );
    });

    /**
     * 資料刪除事件綁定
     */
    $("span.delete").click(function () {
      let deleteId = $(this).parents("tr.record").children("th:eq(0)").text();
      swal({
        title: "確認送出 ?",
        text: "請再次確認要刪除的資料",
        icon: "info",
        buttons: true,
        dangerMode: true,
      }).then((willDelete) => {
        if (willDelete) {
          let res = { err: true };
          deleteData(res, deleteId);
          if (!res["err"]) {
            swal({
              position: "top",
              icon: "success",
              title: "刪除成功",
              button: false,
              timer: 2000,
            });

            loadInfo(curPage, keyword);
          } else {
            swal({
              title: "刪除失敗",
              icon: "warning",
              buttons: true,
              dangerMode: true,
            });
          }
        } else {
          swal({
            title: "未刪除任額資訊",
            icon: "warning",
            buttons: true,
            dangerMode: true,
          });
        }
      });
    });
  }

  /**************************************分頁布局**************************************/

  const page_nav = $("#page_nav");
  page_nav.empty();
  let prevPage = PageNo - 1;
  let nextPage = PageNo + 1;

  page_nav.append(`
      <a href="#" id="first_page" class="page_nav_item" disabled="disabled">第一頁</a>
      <a href="#" id="prev_page" class="page_nav_item" disabled="disabled">上一頁</a>
  `);
  // =================================================頁碼顯示=================================================
  // 顯示5個連續頁數，當前頁數在中間，除當前頁數外，其他頁數都可以跳到指定頁
  // 狀況一: 若總頁數小於5，頁數的範圍是1~總頁數
  // 1頁: 1
  // 2頁: 1 2
  // 3頁: 1 2 3
  // 4頁: 1 2 3 4
  // 5頁: 1 2 3 4 5
  let begin, end;
  if (PageTotal <= 5) {
    begin = 1;
    end = PageTotal;
  }

  if (PageTotal > 5) {
    if (PageNo <= 3) {
      // 狀況二: 總頁數大於5
      // 當前頁數為前3個: 頁數範圍: 1~5
      // 【1】 2 3 4 5
      // 1 【2】3 4 5
      // 1 2 【3】4 5
      begin = 1;
      end = 5;
    } else if (PageNo > PageTotal - 3) {
      // 當前頁數為最後3個8, 9, 10: 頁數範圍: (總頁數-4) ~ 總頁數
      // 6 7 【8】 9 10
      // 6 7 8 【9】 10
      // 6 7 8 9 【10】
      begin = PageTotal - 4;
      end = PageTotal;
    } else {
      // 當前頁數為3, 4, 5, 6, 7: 頁數範圍: (當前頁數-2) ~ (當前頁數+2)
      // 2 3 【4】 5 6
      // 3 4 【5】 6 7
      // 4 5 【6】 7 8
      // 5 6 【7】 8 9
      begin = PageNo - 2;
      end = PageNo + 2;
    }
  }

  for (let i = begin; i <= end; i++) {
    if (i !== PageNo) {
      page_nav.append(
        `<a href="#" class="page_nav_item" id="page_${i}">${i}</a>`
      );
      let prev_page = document.querySelector(`#page_${i}`);
      prev_page.addEventListener("click", (evt) => {
        evt.preventDefault();
        loadInfo(i, keyword);
      });
    } else {
      page_nav.append(`<span class="page_nav_item">【${i}】</span>`);
    }
  }

  // =================================================頁碼顯示=================================================
  page_nav.append(`
      <a href="#" id="next_page" class="page_nav_item" disabled="disabled">下一頁</a>
      <a href="#" id="last_page" class="page_nav_item" disabled="disabled">最末頁</a>
      <span>
            共${PageTotal}頁，${TotalCounts}筆紀錄， 前往第<input value="${PageNo}" type="number" name="pageNo" id="pageNo_input" size="1" min="1" max="${PageTotal}"/> 頁
            <input type="submit" value="Go" id="go_to_page_btn" class="btn btn-info ml-2"/>
      </span>
  `);

  if (prevPage >= 1) {
    // 固定跳到第一頁
    let first_page = document.getElementById("first_page");

    first_page.removeAttribute("disabled");
    first_page.addEventListener("click", (evt) => {
      evt.preventDefault();
      loadInfo(1, keyword);
    });

    let prev_page = document.querySelector("#prev_page");

    prev_page.removeAttribute("disabled");
    prev_page.addEventListener("click", (evt) => {
      evt.preventDefault();
      loadInfo(prevPage, keyword);
    });
  }

  if (nextPage <= PageTotal) {
    let next_page = document.querySelector("#next_page");
    next_page.removeAttribute("disabled");
    next_page.addEventListener("click", (evt) => {
      evt.preventDefault();
      loadInfo(nextPage, keyword);
    });

    // 固定跳到最末頁
    let last_page = document.getElementById("last_page");
    last_page.removeAttribute("disabled");
    last_page.addEventListener("click", (evt) => {
      evt.preventDefault();
      loadInfo(PageTotal, keyword);
    });
  }

  let go_to_page_btn = document.querySelector("#go_to_page_btn");
  let pageNo_input = document.querySelector("#pageNo_input");
  go_to_page_btn.addEventListener("click", (evt) => {
    evt.preventDefault();
    loadInfo(pageNo_input.value, keyword);
  });
}
/**************************************分頁布局**************************************/

// ============================ 關鍵字查詢 ============================
search_btn.addEventListener("click", (evt) => {
  let input = $("#keyword_input").val();
  keyword = input;
  loadInfo(1, keyword);
});
// ============================ 關鍵字查詢 ============================

/**
 * 新增功能的modal送出資料
 */
add_subject_submit_btn.addEventListener("click", (evt) => {
  swal({
    title: "確認送出 ?",
    text: "請再次確認輸入資料",
    icon: "info",
    buttons: true,
    dangerMode: true,
  }).then((willDelete) => {
    if (willDelete) {
      let res = { err: true };
      insertData(res);
      if (!res["err"]) {
        swal({
          position: "top",
          icon: "success",
          title: "資料新增成功",
          button: false,
          timer: 2000,
        });
        addSubjectName.value = "";
        // 查詢總頁數+1
        loadInfo(pages + 1, keyword);
      } else {
        swal({
          title: "新增失敗",
          icon: "warning",
          buttons: true,
          dangerMode: true,
        });
      }
    } else {
      swal({
        title: "未新增任額資訊",
        icon: "warning",
        buttons: true,
        dangerMode: true,
      });
    }
  });
  // 關閉modal
  $("#add_subject_modal").modal("hide");
});

/**
 * 以ajax傳遞新增員工數據
 * @param res: 輸出參數，表示新增成功或失敗
 */
function insertData(res) {
  $.ajax({
    type: "POST",
    url: `${rootPath}/api/subject`,
    data: $("#add_form").serialize(),
    contentType: "application/x-www-form-urlencoded",
    cache: false,
    processData: false,
    async: false,
    success: function (response) {
      const { err, msg } = { ...response };
      res["err"] = err;
    },
    error: function (thrownError) {
      console.log(thrownError);
    },
  });
}

/**
 * 更新功能的modal送出資料
 */
update_subject_submit_btn.addEventListener("click", (EventTarget) => {
  swal({
    title: "確認送出 ?",
    text: "請再次確認更新用資料",
    icon: "info",
    buttons: true,
    dangerMode: true,
  }).then((willDelete) => {
    if (willDelete) {
      let res = { err: true };
      // 修改
      updateData(res);
      if (!res["err"]) {
        swal({
          position: "top",
          icon: "success",
          title: "更新成功",
          button: false,
          timer: 2000,
        });
        $("#update_subject_id").val("");
        $("#update_subject_name").val("");
        loadInfo(curPage, keyword);
      } else {
        swal({
          title: "更新失敗",
          icon: "warning",
          buttons: true,
          dangerMode: true,
        });
      }
    } else {
      swal({
        title: "未更新任額資訊",
        icon: "warning",
        buttons: true,
        dangerMode: true,
      });
    }
  });
  // 關閉modal
  $("#update_subject_modal").modal("hide");
});

/**
 * 以ajax傳遞要修改員工數據
 * @param res: 輸出參數，表示新增成功或失敗
 */
function updateData(res) {
  $.ajax({
    type: "PUT",
    url: `${rootPath}/api/subject`,
    data: $("#update_form").serialize(),
    contentType: "application/x-www-form-urlencoded",
    cache: false,
    processData: false,
    async: false,
    success: function (response) {
      const { err, msg } = { ...response };
      res["err"] = err;
    },
    error: function (thrownError) {
      console.log(thrownError);
    },
  });
}

/**
 * 以ajax傳遞要刪除的員工數據
 * @param res: 輸出參數，表示新增成功或失敗
 * @param id: 要刪除的id
 */
function deleteData(res, deleteId) {
  $.ajax({
    type: "DELETE",
    url: `${rootPath}/api/subject/${deleteId}`,
    async: false,
    success: function (response) {
      const { err, msg } = { ...response };
      res["err"] = err;
    },
    error: function (thrownError) {
      console.log(thrownError);
    },
  });
}

function init() {
  loadInfo(1, keyword);
}

init();
