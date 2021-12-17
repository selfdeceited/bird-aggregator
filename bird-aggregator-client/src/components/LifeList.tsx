import * as Blueprint from '@blueprintjs/core'

import { Button, Icon, Popover } from '@blueprintjs/core'
import React, { useEffect, useState } from 'react'

import { Link } from 'react-router-dom'
import { MapContainer } from './Map/Map'
import { YearlyLifeList } from './YearlyLifeList'
import { fetchLifelist } from '../clients/LifeListClient'
import moment from 'moment'

export interface ILifeListDto {
	birdId: string
	name: string
	dateMet: string
	location: string
	photoId: string
}

export const LifeList: React.FC = () => {
	const [lifelist, setLifelist] = useState<ILifeListDto[]>([])

	const fillLifelist = async (): Promise<void> => {
		const firstOccurences = await fetchLifelist()
		// eslint-disable-next-line @typescript-eslint/no-unsafe-member-access
		setLifelist(firstOccurences)
	}

	useEffect(() => {
		void fillLifelist()
	}, [])

	const LifeListPopover: React.FC<{ photoId: string }> = ({ photoId }) => photoId ?
		(<Blueprint.Popover
			target={<Blueprint.Button className="bp3-button bp3-minimal bp3-icon-map-marker display-block"/>}
			content={<MapContainer embedded photoId={photoId}/>}/>) : null

	return (
		<article className="body lifelist-container">
			<h2 className="show-mobile">Life list</h2>
			<div>
				<Popover>
					<Button>
						<Icon icon="calendar" />&nbsp;&nbsp;&nbsp;Yearly statistics
					</Button>
					<div>
						<YearlyLifeList/>
					</div>
				</Popover>
			</div>
			<table className="bp3-table bp3-striped">
				<thead>
					<tr>
						<th className="hide-mobile">&nbsp;</th>
						<th>Species</th>
						<th>Date</th>
						<th><span className="hide-mobile">Location</span></th>
					</tr>
				</thead>
				<tbody className="life-list-table">
					{
						lifelist.map((x: ILifeListDto, i: number) =>
							(
								<tr key={i}>
									<td className="hide-mobile">{i + 1}</td>
									<td className="bird-column">
										{x.name}
										<Link
											key={x.photoId}
											to={`/photos/${x.photoId}`}
											role="button"
											className="bp3-button bp3-minimal bp3-icon-arrow-right">
										</Link>
									</td>
									<td className="date-column">{moment(x.dateMet).format('YYYY MM DD')}</td>
									<td>
										<span className="hide-mobile">{x.location}&nbsp;&nbsp;</span>
										<LifeListPopover photoId={x.photoId}/>
									</td>
								</tr>
							))
					}
				</tbody>
			</table>
		</article>)
}
