import React, { useEffect, useState } from 'react'

import { fetchYearlyLifelist } from '../clients/LifeListClient'

export interface IYearlyLifeListDto {
	key: number
	count: number
}

export const YearlyLifeList: React.FC = () => {
	const [lifelist, setLifelist] = useState<IYearlyLifeListDto[]>([])
	useEffect(() => {
		void (async () => {
			const perYearCollection = await fetchYearlyLifelist()
			setLifelist(perYearCollection)
		})()
	}, [])

	return (
		<div>
			<table className="bp3-table bp3-striped yearly-lifelist-container">
				<thead>
					<tr>
						<th className="hide-mobile">Year</th>
						<th>Count</th>
					</tr>
				</thead>
				<tbody className="life-list-table">
					{
						lifelist.map((x: IYearlyLifeListDto, i: number) =>
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
