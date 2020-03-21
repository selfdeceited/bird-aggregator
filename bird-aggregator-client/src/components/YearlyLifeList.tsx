import * as axios from "../http.adapter"
import * as React from "react"

interface IYearlyLifeListProps {}
interface IYearlyLifeListState {
    lifeList: IYearlyLifeListDto[]
}

interface IYearlyLifeListDto {
    key: number,
    count: number
}

export class YearlyLifeList extends React.Component<IYearlyLifeListProps, IYearlyLifeListState> {
    constructor(props: IYearlyLifeListProps) {
        super(props)
        this.state = {
            lifeList: [],
        }
    }
    public componentDidMount() {
        axios.get(`/api/birds/lifelist/peryear`).then(res => {
            const lifeList = res.data as IYearlyLifeListDto[]
            this.setState({ lifeList })
        })
    }

    public render() {
        return (
<div>
    <table className="bp3-table bp3-striped">
        <thead>
            <tr>
                <th className="hide-mobile">Year</th>
                <th>Count</th>
            </tr>
        </thead>
        <tbody className="life-list-table">
            {
                this.state.lifeList.map((x: IYearlyLifeListDto, i: number) =>
                (
                    <tr key={i}>
                        <td>
                            {x.key}
                        </td>
                        <td>{x.count}</td>
                    </tr>
                ))
            }
        </tbody>
    </table>
</div>)
    }
}
