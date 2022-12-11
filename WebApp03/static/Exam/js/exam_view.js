const rootPath = `${window.location.origin}/`;
const pageSize = 5;
let pages = 1;
let curPage = 1;
let subjectId = 1;
let keyword = "";

const add_exam_submit_btn = document.getElementById("add_exam_submit_btn");
const update_exam_submit_btn = document.getElementById(
  "update_exam_submit_btn"
);

/**
 * 將數字以特定位數顯示，缺位向左側補0
 * 如: 000001
 * @param num: number
 * @param length
 * @return num: string
 */
function paddingZero(num, length) {
  if ((num + "").length >= length) {
    return num;
  }
  return paddingZero("0" + num, length);
}

/**
 * 提取員工編號
 * @param word
 * @returns {*}
 */
function extractEmpNo(word) {
  let endIndex = word.indexOf("_");
  return word.slice(0, endIndex);
}

function loadOptions() {
  $.ajax({
    type: "GET",
    url: `${rootPath}/api/subject/getAll`,
    async: false,
    success: function (rsp) {
      const subjects = [];
      subjects.push(...rsp);
      createOption(subjects);
    },
    error: function (thrownError) {
      console.log(thrownError);
    },
  });
}

function createOption(subjectList) {
  for (let i = 0; i < subjectList.length; i++) {
    const subjectOptions = $("#subject_options");
    let item = subjectList[i];
    if (i === 0) {
      subjectOptions.append(
        `<option value="${item.Id}" selected>${item.Name}</option>`
      );
    } else {
      subjectOptions.append(`<option value="${item.Id}">${item.Name}</option>`);
    }
  }
}

$("#subject_options").change(function (evt) {
  let subjectIdVal = $("#subject_options :selected").val();
  if (subjectIdVal != 0) {
    // 排除提醒用選項
    subjectId = subjectIdVal;

    loadInfo(1, subjectId);
  }
});

/**
 * 加載頁面訊息
 */
function loadInfo(page, subjectId) {
  $.ajax({
    type: "GET",
    url: `${rootPath}/api/exam/${page}?subjectId=${subjectId}`,
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
 * 創建未登記成績的員工名稱選單
 * @param validEmpList
 */
function createValidEmpList(validEmpList) {
  $("#add_emp_no").empty();
  for (const item of validEmpList) {
    $("#add_emp_no").append(
      `<option value="${item.EmpNo}" >${item.EmpNo}_${item.Name}</option>`
    );
  }
}

function loadValidEmpList(subjectId) {
  $.ajax({
    type: "GET",
    url: `${rootPath}/api/exam/get_emp_list/${subjectId}`,
    dataType: "json",
    async: false,
    success: function (rsp) {
      createValidEmpList(rsp);
    },
    error: function (thrownError) {
      console.log(thrownError);
    },
  });
}

/**
 * 畫面中展示成績訊息
 * @param emps: array
 */
function display(Items, PageNo, PageTotal, TotalCounts) {
  pages = PageTotal;
  curPage = PageNo;
  const tbody = $("tbody");
  tbody.empty();
  if (TotalCounts === 0) {
    tbody.append(
      `<th colspan="7" style="color: palevioletred">未找到符合條件的結果</th>>`
    );
  } else {
    let preRank = 0;
    let preScore = 0;

    for (let i = 0; i < Items.length; i++) {
      let exam = Items[i];

      let rank = i + 1 + (curPage - 1) * pageSize;
      let score = exam.Score;
      if (preScore == score) {
        rank = preRank;
      }
      tbody.append(`
      <tr class="record">
          <th scope="row" style="display: none">${exam.Id}</th>
          <th scope="row" style="display: none">${exam.SubjectId}_${exam.SubjectName}</th>
          <th scope="row" style="display: none">${exam.EmpNo}_${exam.EmpName}</th>
          <td>${rank}</td>
          <td>${exam.EmpName}</td>
          <td>${exam.Score}</td>
          <td>
              <button type="button" class="btn btn-light" data-target="#update_exam_modal" data-toggle="modal">
                  <span class="update">✏</span>
              </button>
              <button type="button" class="btn btn-light" >
                  <span class="delete">❌</span>
              </button>
          </td>
      </tr>
    `);
      preRank = rank;
      preScore = score;
    }

    /**
     * 更新按鈕事件綁定
     * 查找所要更新數據的資訊，並將資訊寫到更新用的modal
     */
    $("span.update").click(function () {
      // 清空上次填寫欄位的值
      $("#update_form").children("input").val("");
      // 讀取tr中的考試編號，寫到modal
      $("#update_exam_id").val(
        $(this).parents("tr.record").children("th:eq(0)").text()
      );
      // 讀取tr中的員工編號，寫到modal
      $("#update_emp_no").val(
        $(this).parents("tr.record").children("th:eq(2)").text()
      );
      // 讀取tr中的科目，寫到modal
      $("#update_subject_name").val(
        $(this).parents("tr.record").children("th:eq(1)").text()
      );
      // 讀取tr中的分數，寫到modal
      $("#update_score").val(
        $(this).parents("tr.record").children("td:eq(2)").text()
      );
    });

    /**
     * 資料刪除事件綁定
     */
    $("span.delete").click(function () {
      //
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

            loadInfo(curPage, subjectId);
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

  // 綁定添加modal的科目id
  $("#add_subject_id").val(subjectId);
  loadValidEmpList(subjectId);
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
        loadInfo(i, subjectId);
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
      loadInfo(1, subjectId);
    });

    let prev_page = document.querySelector("#prev_page");

    prev_page.removeAttribute("disabled");
    prev_page.addEventListener("click", (evt) => {
      evt.preventDefault();
      loadInfo(prevPage, subjectId);
    });
  }

  if (nextPage <= PageTotal) {
    let next_page = document.querySelector("#next_page");
    next_page.removeAttribute("disabled");
    next_page.addEventListener("click", (evt) => {
      evt.preventDefault();
      loadInfo(nextPage, subjectId);
    });

    // 固定跳到最末頁
    let last_page = document.getElementById("last_page");
    last_page.removeAttribute("disabled");
    last_page.addEventListener("click", (evt) => {
      evt.preventDefault();
      loadInfo(PageTotal, subjectId);
    });
  }

  let go_to_page_btn = document.querySelector("#go_to_page_btn");
  let pageNo_input = document.querySelector("#pageNo_input");
  go_to_page_btn.addEventListener("click", (evt) => {
    evt.preventDefault();
    loadInfo(pageNo_input.value, subjectId);
  });
  /**************************************分頁布局**************************************/
}

/**
 * 新增功能的modal送出資料
 */
add_exam_submit_btn.addEventListener("click", (evt) => {
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
        $("#add_score").val("");
        // 查詢總頁數+1
        loadInfo(pages + 1, subjectId);
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
  $("#add_exam_modal").modal("hide");
});

/**
 * 以ajax傳遞新增成績數據
 * @param res: 輸出參數，表示新增成功或失敗
 */
function insertData(res) {
  $.ajax({
    type: "POST",
    url: `${rootPath}/api/exam`,
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

update_exam_submit_btn.addEventListener("click", (EventTarget) => {
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
        $("#update_emp_no").val("");
        $("#update_subject_name").val("");
        $("#update_score").val("");
        loadInfo(curPage, subjectId);
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
  $("#update_exam_modal").modal("hide");
});

/**
 * 以ajax傳遞要修改考試的數據
 * @param res: 輸出參數，表示新增成功或失敗
 */
function updateData(res) {
  $.ajax({
    type: "PUT",
    url: `${rootPath}/api/exam`,
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
 * 以ajax傳遞要刪除的考試數據
 * @param res: 輸出參數，表示新增成功或失敗
 * @param id: 要刪除的id
 */
function deleteData(res, deleteId) {
  $.ajax({
    type: "DELETE",
    url: `${rootPath}/api/exam/${deleteId}`,
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
  loadOptions();
  loadInfo(1, 1);
}

init();
