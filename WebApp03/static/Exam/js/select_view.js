﻿const rootPath = `${window.location.origin}/`;
const pageSize = 5;
let pages = 1;
let curPage = 1;
let empId = 38;

function loadOptions() {
  $.ajax({
    type: "GET",
    url: `${rootPath}/api/emp/getAll`,
    async: false,
    success: function (rsp) {
      const empList = [];
      empList.push(...rsp);
      createOption(empList);
    },
    error: function (thrownError) {
      console.log(thrownError);
    },
  });
}

function createOption(empList) {
  for (let i = 0; i < empList.length; i++) {
    const empOptions = $("#emp_options");
    let item = empList[i];
    if (i === 0) {
      empOptions.append(
        `<option value="${item.Id}" selected>${item.EmpNo}_${item.Name}</option>`
      );
    } else {
      empOptions.append(
        `<option value="${item.Id}">${item.EmpNo}_${item.Name}</option>`
      );
    }
  }
}

/**
 * 綁定員工選單事件
 */
$("#emp_options").change(function (evt) {
  let empIdVal = $("#emp_options :selected").val();
  if (empIdVal != 0) {
    // 排除提醒用選項
    empId = empIdVal;

    loadInfo(1, empId);
  }
});

/**
 * 加載頁面成績資訊
 * @param {int} page: 當前頁數
 * @param {int} empId: 員工編號
 */
function loadInfo(page, empId) {
  $.ajax({
    type: "GET",
    url: `${rootPath}/api/exam/get_by_emp_id/${page}/${empId}`,
    dataType: "json",
    async: false,
    success: function (rsp) {
      const { Items, PageNo, PageTotal, TotalCounts } = rsp;
      display(Items, PageNo, PageTotal, TotalCounts);
    },
    error: function (thrownError) {
      console.log(thrownError);
    },
  });
}

/**
 *
 * @param {Object} Items: 頁面資訊
 * @param {int} PageNo: 當前頁數
 * @param {int} PageTotal: 總頁數
 * @param {int} TotalCounts: 總資料筆數
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
    for (let i = 0; i < Items.length; i++) {
      let exam = Items[i];
      tbody.append(`
      <tr class="record">
          <th scope="row" >${exam.SubjectName}</th>
          <td>${exam.Rank}/${exam.TotalNum}</td>
          <td>${exam.Score}</td>
      </tr>
    `);
    }
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
        loadInfo(i, empId);
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
      loadInfo(1, empId);
    });

    let prev_page = document.querySelector("#prev_page");

    prev_page.removeAttribute("disabled");
    prev_page.addEventListener("click", (evt) => {
      evt.preventDefault();
      loadInfo(prevPage, empId);
    });
  }

  if (nextPage <= PageTotal) {
    let next_page = document.querySelector("#next_page");
    next_page.removeAttribute("disabled");
    next_page.addEventListener("click", (evt) => {
      evt.preventDefault();
      loadInfo(nextPage, empId);
    });

    // 固定跳到最末頁
    let last_page = document.getElementById("last_page");
    last_page.removeAttribute("disabled");
    last_page.addEventListener("click", (evt) => {
      evt.preventDefault();
      loadInfo(PageTotal, empId);
    });
  }

  let go_to_page_btn = document.querySelector("#go_to_page_btn");
  let pageNo_input = document.querySelector("#pageNo_input");
  go_to_page_btn.addEventListener("click", (evt) => {
    evt.preventDefault();
    loadInfo(pageNo_input.value, empId);
  });
  /**************************************分頁布局**************************************/
}

function init() {
  loadOptions();
  loadInfo(1, empId);
}

init();
