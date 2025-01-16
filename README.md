
# MudComposite

MudBlazor를 확장하여 재사용 가능하고 유연한 컴포넌트 기반 설계를 지원하는 라이브러리.

---

## 프로젝트 개요

`MudComposite`는 **MudBlazor**의 기능을 확장하여 개발자가 복잡한 UI를 더욱 쉽게 관리할 수 있도록 설계된 라이브러리입니다.  
`Composite Pattern`을 기반으로 다양한 데이터 유형에 대해 리스트 및 상세 보기를 제공하며, 비즈니스 로직과 UI 로직의 결합도를 낮추는 구조를 제공합니다.

---

## 주요 기능

### 1. 컴포넌트 확장
- **MudDataGridComposite**: MudBlazor 데이터 그리드를 확장하여 재사용 가능한 리스트 컴포넌트 제공.
- **MudDetailViewComposite**: 상세 보기 컴포넌트를 제공하며, 데이터 검색, 추가 및 수정 기능 지원.

### 2. 인터페이스 기반 설계
- `IMudDataGridComposite`, `IMudDetailViewComposite`을 통해 표준화된 컴포넌트 동작 정의.
- 다양한 데이터 모델과 사용 사례에 쉽게 확장 가능.

### 3. 비동기 작업 관리
- `OnServerReload`, `OnRetrieve`, `OnSubmit` 등의 이벤트를 활용하여 비동기 데이터 처리 지원.
- 사용자 경험 개선을 위한 진행 상태 표시(`ProgressDialog`).

---

## 주요 클래스 및 인터페이스

### 1. `IMudDetailViewComposite`
- 상세 보기 컴포넌트를 위한 인터페이스로 데이터 검색 및 제출 이벤트를 정의.
- 주요 메서드 및 속성:
  - `Func<Task<Results>> OnRetrieve`
  - `Func<Task<Results>> OnSubmit`
  - `TRetrieved RetrieveItem`

### 2. `IMudDataGridComposite`
- 데이터 그리드 컴포넌트를 위한 인터페이스로 데이터 로드, 삭제 이벤트를 정의.
- 주요 메서드 및 속성:
  - `Func<GridState<TModel>, Task<GridData<TModel>>> OnServerReload`
  - `Func<TModel, Task<Results>> OnRemove`
  - `Task<GridData<TModel>> ServerReload(GridState<TModel> state)`
  - `TModel SelectedItem`
  - `TSearchModel SearchModel`

### 3. `MudDataGridComposite`
- `IMudDataGridComposite`를 구현한 추상 클래스.
- 데이터 그리드와 관련된 이벤트 및 동작(행 스타일, 서버 데이터 로드, 삭제 등)을 정의.

### 4. `MudDetailViewComposite`
- `IMudDetailViewComposite`를 구현한 추상 클래스.
- 데이터 검색 및 저장 작업을 정의하며, 비즈니스 로직과 UI를 연결.

---

## 설치 및 사용법

### 1. NuGet 설치
```bash
dotnet add package MudComposite
```

### 2. 사용법 예제

#### 데이터 그리드 컴포넌트
```csharp
public class WeatherDataGridComposite : MudDataGridComposite<WeatherForecast, SearchModel>, IWeatherListViewComposite
{
    private readonly IWeatherService _weatherService;

    public WeatherDataGridComposite(IDialogService dialogService, ISnackbar snackbar, NavigationManager navigationManager, IWeatherService weatherService)
        : base(dialogService, snackbar, navigationManager)
    {
        _weatherService = weatherService;
    }

    public override void Initialize()
    {
        this.OnServerReload = async (state) =>
        {
            var result = await _weatherService.GetList(this.SearchModel, state.Page, state.PageSize);
            return new GridData<WeatherForecast>()
            {
                TotalItems = result.TotalCount,
                Items = result.Datum
            };
        };
    }
}
```

#### 상세 보기 컴포넌트
```csharp
public class WeatherDetailComposite : MudDetailViewComposite<WeatherForecast>, IWeatherDetailComposite
{
    private readonly IWeatherService _weatherService;

    public WeatherDetailComposite(IDialogService dialogService, ISnackbar snackbar, IWeatherService weatherService)
        : base(dialogService, snackbar)
    {
        _weatherService = weatherService;
    }

    public override void Initialize()
    {
        this.OnRetrieve = async () => await _weatherService.Get(this.RetrieveItem.Id);
        this.OnSubmit = async () =>
        {
            if (this.RetrieveItem.Id > 0)
            {
                return await _weatherService.Modify(this.RetrieveItem);
            }
            return await _weatherService.Add(this.RetrieveItem);
        };
    }
}
```

---

## 설계 개요

- **Composite Pattern**: 데이터와 UI 로직 간의 결합도를 낮추고 재사용성을 극대화.
- **Interface 기반 설계**: 표준화된 동작 정의로 확장성과 유지보수성 강화.
- **MudBlazor 활용**: 최신 UI 프레임워크의 강력한 기능을 기반으로 설계.

---

## 기여 방법

1. 이 프로젝트를 포크합니다.
2. 새로운 브랜치를 생성합니다.
   ```bash
   git checkout -b feature/YourFeatureName
   ```
3. 변경 사항을 커밋합니다.
   ```bash
   git commit -m "Add some feature"
   ```
4. 브랜치에 푸시합니다.
   ```bash
   git push origin feature/YourFeatureName
   ```
5. Pull Request를 생성합니다.

---

## 라이선스

이 프로젝트는 MIT 라이선스 하에 배포됩니다. 자세한 내용은 [LICENSE](LICENSE) 파일을 확인하세요.

---

## 문의

질문이나 문제점이 있다면 [Issues](https://github.com/your-repo/MudComposite/issues) 섹션에 작성해주세요.
